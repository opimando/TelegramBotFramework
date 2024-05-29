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
        MessageId? replayTo = null
    )
    {
        Id = id;
        Content = content;
        ChatId = chatId;
        From = from;
        ReplyTo = replayTo;
    }

    public MessageId Id { get; }
    public IMessageContent Content { get; }
    public ChatId ChatId { get; }
    public User From { get; }
    public MessageId? ReplyTo { get; }
    public DateTime Created { get; } = DateTime.Now;

    public override string ToString()
    {
        return $"Сообщение от {From}: {Content}";
    }
}