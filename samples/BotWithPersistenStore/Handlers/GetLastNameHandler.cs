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

public class GetLastNameHandler : BaseChatState, IChatStateWithData<NameArgument>
{
    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        _argument.MessagesIds.Add(receivedMessage.Id);
        await Messenger.Send(receivedMessage.ChatId,
            $"Твоё имя: '{_argument.FirstName}', фамилия: '{(receivedMessage.Content as TextContent)!.Content}'");

        return new StateInfo(null);
    }

    protected override async Task OnStateStartInternal(ChatId chatId)
    {
        _argument ??= new NameArgument();
        _argument.MessagesIds.Add(await Messenger.Send(chatId, "Введи фамилию"));
    }

    protected override async Task OnStateExitInternal(ChatId chatId)
    {
        if (_argument == null) return;

        foreach (MessageId message in _argument.MessagesIds)
            await Messenger.Delete(chatId, message);
    }

    public NameArgument GetData()
    {
        return _argument;
    }

    public Task SetData(NameArgument data)
    {
        _argument = data;
        return Task.CompletedTask;
    }

    private NameArgument _argument = new();
}

public class NameArgument : MessageToDeleteArgument
{
    public string FirstName { get; set; } = string.Empty;
}