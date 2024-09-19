#region Copyright

/*
 * File: MessageUpdatedEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class MessageUpdatedEvent : MessageSendEvent
{
    public MessageId OldMessageId { get; }

    public MessageUpdatedEvent(ChatId destination, SendInfo message, MessageId oldMessageId) : base(destination,
        message)
    {
        OldMessageId = oldMessageId;
    }

    public override string Template => "Сообщение {OldMessageId} изменено в чате {Destination}";
    public override object[] Items => new object[] {OldMessageId, Destination};
}