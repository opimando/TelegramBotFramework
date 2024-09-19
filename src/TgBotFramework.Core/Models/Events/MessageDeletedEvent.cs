#region Copyright

/*
 * File: MessageDeletedEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class MessageDeletedEvent : BaseEvent, IStructuredEvent
{
    public MessageDeletedEvent(MessageId deletedMessageId)
    {
        DeletedMessageId = deletedMessageId;
    }

    public MessageId DeletedMessageId { get; }

    public LogLevel Level => LogLevel.Debug;
    public string Template => "Сообщение {DeletedMessageId} удалено";
    public object[] Items => new object[] {DeletedMessageId};
}