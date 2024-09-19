#region Copyright

/*
 * File: BaseSelfDeleteMessagesState.cs
 * Author: denisosipenko
 * Created: 2023-12-21
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Базовая реализация обработчика, который в конце удаляет все сообщения, которые помещены в Data.
/// </summary>
public abstract class BaseSelfDeleteMessagesState : BaseChatState, IChatStateWithData<MessageToDeleteArgument>
{
    protected MessageToDeleteArgument Data = new();

    public virtual MessageToDeleteArgument GetData()
    {
        return Data;
    }

    public virtual Task SetData(MessageToDeleteArgument data)
    {
        Data = data;
        return Task.CompletedTask;
    }

    protected override async Task OnStateExitInternal(ChatId chatId)
    {
        foreach (MessageId messageId in Data.MessagesIds)
            await Messenger.Delete(chatId, messageId);
    }
}