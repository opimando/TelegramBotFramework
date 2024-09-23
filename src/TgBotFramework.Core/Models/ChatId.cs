#region Copyright

/*
 * File: ChatId.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ChatId
{
    public long Id { get; }

    public ChatId(long id)
    {
        Id = id;
    }

    public static implicit operator ChatId(long id)
    {
        return new ChatId(id);
    }

    public static implicit operator long(ChatId chatId)
    {
        return chatId.Id;
    }

    public override string ToString()
    {
        return Id.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ChatId chatId) return base.Equals(obj);

        return Id.Equals(chatId.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}