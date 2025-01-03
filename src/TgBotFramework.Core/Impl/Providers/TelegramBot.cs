﻿#region Copyright

/*
 * File: TelegramBot.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBotFramework.Core;

public class TelegramBot : ITelegramBot
{
    private readonly ITelegramBotClient _client;
    private readonly IEventBus _eventBus;
    private readonly IMessageProcessQueue _messageQueue;
    private readonly IAuthProvider? _authProvider;
    private readonly ExceptionPolicy? _exceptionPolicy;

    public TelegramBot(ITelegramBotClient client, IEventBus eventBus, IMessageProcessQueue messageQueue,
        IAuthProvider? authProvider = null, ExceptionPolicy? exceptionPolicy = null)
    {
        _client = client;
        _eventBus = eventBus;
        _messageQueue = messageQueue;
        _authProvider = authProvider;
        _exceptionPolicy = exceptionPolicy;
    }

    public void Start()
    {
        _client.StartReceiving(UpdateHandler, PollingErrorHandler);
        _eventBus.Publish(new BotStartedEvent());
    }

    public void Stop()
    {
        _eventBus.Publish(new BotStoppedEvent());
    }

    private Task PollingErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        _eventBus.Publish(new TelegramErrorEvent(arg2));
        return Task.CompletedTask;
    }

    private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancel)
    {
        string messageMeta = string.Empty;
        try
        {
            messageMeta = (update.Message?.MessageId ?? update.Id).ToString() + update.Message?.Chat.Id;
            _eventBus.Publish(new ReceivedUpdateEvent(messageMeta));

            Message? receivedMessage = update.GetMessage(client);

            if (receivedMessage == null)
            {
                _eventBus.Publish(new CantReadMessageEvent(messageMeta));
                return;
            }

            _eventBus.Publish(new ReceivedMessageEvent(receivedMessage.ChatId, receivedMessage.From,
                receivedMessage.Content));

            if (_authProvider != null)
                if (!await _authProvider.HasAccess(receivedMessage.From))
                {
                    await NotifyAboutAccessDenies(receivedMessage);
                    return;
                }

            _messageQueue.Enqueue(receivedMessage.ChatId, receivedMessage);
        }
        catch (Exception ex)
        {
            if (update?.Message?.Chat?.Id != default && _exceptionPolicy != null)
            {
                if (_exceptionPolicy.SendToUser)
                {
                    try
                    {
                        string message = _exceptionPolicy.ExceptionHandler == null
                            ? ex.Message
                            : _exceptionPolicy.ExceptionHandler(ex);
                        
                        if (!string.IsNullOrWhiteSpace(message))
                            await client.SendMessage(new Telegram.Bot.Types.ChatId(update.Message.Chat.Id), message,
                                cancellationToken: cancel);
                    }
                    catch (Exception ex2)
                    {
                        _eventBus.Publish(new ErrorEvent(ex, "Ошибка при отправке исключения: " + ex2.Message));
                    }
                }
            }
              
            _eventBus.Publish(new ErrorEvent(ex, "Ошибка при обработке нового сообщения: " + messageMeta));
        }
    }

    public async Task SetCommands(IEnumerable<CommandButton> buttons)
    {
        var buttonsList = buttons.ToList();
        var tgCommands = buttonsList
            .Select(s => s.ToCommandButton())
            .Where(s => s != null)
            .Select(s => s!).ToArray();

        await _client.SetMyCommands(tgCommands);
        _eventBus.Publish(new SetBotCommandsEvent(buttonsList));
    }

    public async Task SetDescription(string description)
    {
        await _client.SetMyDescription(description);
        _eventBus.Publish(new SetBotDescriptionEvent(description));
    }

    private async Task NotifyAboutAccessDenies(Message message)
    {
        string textToMessage = _authProvider == null ? "Нет доступа" : await _authProvider.GetAccessDeniedMessage();
        await _client.SendMessage(message.ChatId.Id, textToMessage);
        _eventBus.Publish(new UserHasNotAccessEvent(message.From));
    }
}