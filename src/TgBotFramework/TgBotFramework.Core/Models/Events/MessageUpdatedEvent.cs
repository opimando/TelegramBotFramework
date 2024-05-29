#region Copyright

/*
 * File: MessageUpdatedEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class MessageUpdatedEvent : MessageSendEvent, IStructuredEvent
{
    public MessageId OldMessageId { get; }

    public MessageUpdatedEvent(ChatId destination, SendInfo message, MessageId oldMessageId) : base(destination,
        message)
    {
        OldMessageId = oldMessageId;
    }

    public string Template => "Сообщение {OldMessageId} изменено в чате {Destination}";
    public object[] Items => new object[] {OldMessageId, Destination};
}