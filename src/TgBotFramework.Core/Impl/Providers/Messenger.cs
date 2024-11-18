#region Copyright

/*
 * File: Messenger.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBotFramework.Core;

public class Messenger : IMessenger
{
    private readonly ISpamSenderFilter? _spamFilter;
    private readonly IEventBus _eventBus;

    public ITelegramBotClient Client { get; }

    public Messenger(ITelegramBotClient client, IEventBus eventBus, ISpamSenderFilter? spamFilter = null)
    {
        Client = client;
        _spamFilter = spamFilter;
        _eventBus = eventBus;
    }

    private async Task<MessageId> InternalSendMessage(ChatId chatId, SendInfo sendMessageInfo,
        MessageId? replyTo = null)
    {
        ThrowIfMessageIsEmpty(sendMessageInfo.Content);

        await WaitSpamFilterIfExist();

        IReplyMarkup? markup = GetMarkup(sendMessageInfo);

        MessageId messageId = sendMessageInfo.Content switch
        {
            ChatActionContent action => await action.Send(Client, chatId),
            TextContent text => await text.Send(Client, chatId, sendMessageInfo, markup, replyTo),
            ImageGroupContent group => await group.Send(Client, chatId, sendMessageInfo, replyTo),
            LocationContent location => await location.Send(Client, chatId, sendMessageInfo, markup, replyTo),
            ContactContent contact => await contact.Send(Client, chatId, sendMessageInfo, markup, replyTo),
            BaseFileContent file => await file.Send(Client, chatId, sendMessageInfo, markup, replyTo),
            _ => throw new ArgumentOutOfRangeException("Content",
                $"Отправка типа сообщения {sendMessageInfo.Content.GetType().Name} не поддерживается")
        };

        _eventBus.Publish(new MessageSendEvent(chatId, sendMessageInfo));
        return messageId;
    }

    public Task<MessageId> Send(ChatId chatId, SendInfo sendMessageInfo)
    {
        return InternalSendMessage(chatId, sendMessageInfo);
    }

    public Task<MessageId> Reply(ChatId chatId, MessageId replyTo, SendInfo sendMessageInfo)
    {
        return InternalSendMessage(chatId, sendMessageInfo, replyTo);
    }

    public async Task Edit(ChatId chatId, MessageId messageToEditId, SendInfo updatedSendMessageInfo)
    {
        ThrowIfMessageIsEmpty(updatedSendMessageInfo.Content);
        await WaitSpamFilterIfExist();

        IReplyMarkup? markup = GetMarkup(updatedSendMessageInfo);
        InlineKeyboardMarkup? inlineMarkup = null;

        if (markup != null)
        {
            if (markup is InlineKeyboardMarkup inline)
                inlineMarkup = inline;
            else
                throw new InvalidCastException("Невозможно обновить не InlineKeyboard сообщение");
        }

        switch (updatedSendMessageInfo.Content)
        {
            case TextContent text:
                await InternalEditMessage(text, messageToEditId, chatId, updatedSendMessageInfo, inlineMarkup);
                break;
            default:
                throw new ArgumentOutOfRangeException("Content",
                    $"Тип сообщения {updatedSendMessageInfo.Content.GetType().Name} не поддерживается");
        }

        _eventBus.Publish(new MessageUpdatedEvent(chatId, updatedSendMessageInfo, messageToEditId));
    }

    private async Task InternalEditMessage(
        TextContent text,
        MessageId messageToEditId,
        ChatId chatId,
        SendInfo sendInfo,
        InlineKeyboardMarkup? markup)
    {
        string newText = text.Content.GetMessageText();
        await Client.EditMessageText(
            chatId.Id,
            messageToEditId,
            newText,
            sendInfo.ParseMode.GetParseMode(),
            replyMarkup: markup
        );
    }

    public async Task Delete(ChatId chatId, MessageId messageId)
    {
        try
        {
            await Client.DeleteMessage(chatId.Id, messageId);
            _eventBus.Publish(new MessageDeletedEvent(messageId));
        }
        catch (Exception ex)
        {
            _eventBus.Publish(new ErrorEvent(ex, "Ошибка при удалении сообщения"));
        }
    }

    private void ThrowIfMessageIsEmpty(IMessageContent messageContent)
    {
        if (messageContent.IsEmpty())
            throw new ArgumentException("Сообщение не может быть пустым", nameof(messageContent));
    }

    private IReplyMarkup? GetMarkup(SendInfo message)
    {
        if (message.Buttons == null) return null;
        if (message.Buttons is not BaseMessageButtonGroup group)
            throw new InvalidCastException("Не удалось привести схему кнопок к доступной схеме");

        return message.Buttons.ToMarkup();
    }

    private Task WaitSpamFilterIfExist()
    {
        if (_spamFilter == null) return Task.CompletedTask;

        return _spamFilter.WaitIfLimitHit();
    }
}