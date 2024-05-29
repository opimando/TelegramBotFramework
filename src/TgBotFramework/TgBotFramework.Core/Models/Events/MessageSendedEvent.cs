#region Copyright

/*
 * File: MessageSendEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class MessageSendEvent : BaseEvent, IStructuredEvent
{
    public SendInfo Message { get; }
    public ChatId Destination { get; }

    public MessageSendEvent(ChatId destination, SendInfo message)
    {
        Destination = destination;
        Message = message;
    }

    public string Template => "Сообщение {Message} отправлено в чат {Destination}";
    public object[] Items => new object[] {Message, Destination};
}