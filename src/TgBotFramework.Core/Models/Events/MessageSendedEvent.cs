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
    public TimeSpan DurationTime { get; }

    public MessageSendEvent(ChatId destination, SendInfo message, TimeSpan duration)
    {
        Destination = destination;
        Message = message;
        DurationTime = duration;
    }

    public virtual LogLevel Level => LogLevel.Debug;
    public virtual string Template => "Сообщение {@Message} отправлено в чат {Destination} за {DurationMs} мс";
    public virtual object[] Items => new object[] {Message, Destination, DurationTime.TotalMilliseconds};
}