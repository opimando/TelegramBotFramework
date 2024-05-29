#region Copyright

/*
 * File: GetLastnameHandler.cs
 * Author: denisosipenko
 * Created: 2024-04-29
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

using TgBotFramework.Core;

namespace BotWithPersistenStore.Handlers;

public class GetLastNameHandler : BaseChatState, IChatStateWithData<LastnameArgument>
{
    public GetLastNameHandler(IEventBus eventsBus) : base(eventsBus)
    {
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        _argument.MessagesIds.Add(receivedMessage.Id);
        await messenger.Send(receivedMessage.ChatId,
            $"Твоё имя: '{_argument.FirstName}', фамилия: '{(receivedMessage.Content as TextContent).Content}'");

        return null;
    }

    protected override async Task OnStateStartInternal(IMessenger messenger, ChatId chatId)
    {
        _argument ??= new LastnameArgument();
        _argument.MessagesIds.Add(await messenger.Send(chatId, "Введи фамилию"));
    }

    protected override async Task OnStateExitInternal(IMessenger messenger, ChatId chatId)
    {
        if (_argument == null) return;

        foreach (MessageId message in _argument.MessagesIds)
            await messenger.Delete(chatId, message);
    }

    public LastnameArgument GetData()
    {
        return _argument;
    }

    public Task SetData(LastnameArgument data)
    {
        _argument = data;
        return Task.CompletedTask;
    }

    private LastnameArgument _argument = new();
}

public class LastnameArgument : MessageToDeleteArgument
{
    public string FirstName { get; set; }
}