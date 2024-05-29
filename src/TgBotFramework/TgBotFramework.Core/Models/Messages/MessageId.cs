#region Copyright

/*
 * File: MessageId.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class MessageId
{
    public int Id { get; }

    public MessageId(int id)
    {
        Id = id;
    }

    public static implicit operator MessageId(int id)
    {
        return new MessageId(id);
    }

    public static implicit operator int(MessageId messageId)
    {
        return messageId.Id;
    }

    public override string ToString()
    {
        return Id.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not MessageId messageId) return base.Equals(obj);

        return Id.Equals(messageId.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static MessageId NotExistId => new(-1);
}