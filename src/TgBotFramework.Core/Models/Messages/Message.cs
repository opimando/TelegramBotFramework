#region Copyright

/*
 * File: Message.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class Message
{
    public Message(
        MessageId id,
        IMessageContent content,
        ChatId chatId,
        User from,
        Message? replayTo = null,
        User? forwardedFrom = null
    )
    {
        Id = id;
        Content = content;
        ChatId = chatId;
        From = from;
        ReplyTo = replayTo;
        ForwardedFrom = forwardedFrom;
    }

    public MessageId Id { get; }
    public IMessageContent Content { get; }
    public ChatId ChatId { get; }
    public User From { get; }
    public Message? ReplyTo { get; }
    public User? ForwardedFrom { get; }
    public DateTime Created { get; } = DateTime.Now;
    public DateTime? ProcessedTime { get; set; }

    public override string ToString()
    {
        return $"Сообщение от {From}: {Content}";
    }
}