#region Copyright

/*
 * File: TelegramQueryStateAttribute.cs
 * Author: denisosipenko
 * Created: 2024-01-16
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

[AttributeUsage(AttributeTargets.Class)]
public class TelegramQueryStateAttribute : TelegramStateAttribute
{
    /// <summary>
    /// В Inline запросе обязательно должно присутсвовать хотя бы одно ключевое слово из accpetedMessages 
    /// </summary>
    public bool OnlyWithKey { get; }

    public TelegramQueryStateAttribute(bool needIgnore, bool hasCustomAccessFunction,
        bool onlyWithKey,
        params string[] acceptedMessages) :
        base(needIgnore, hasCustomAccessFunction, acceptedMessages)
    {
        OnlyWithKey = onlyWithKey;
    }

    public TelegramQueryStateAttribute(bool hasCustomAccessFunction, bool onlyWithKey, params string[] acceptedMessages)
        : base(
            hasCustomAccessFunction, acceptedMessages)
    {
        OnlyWithKey = onlyWithKey;
    }

    public TelegramQueryStateAttribute(bool onlyWithKey, params string[] acceptedMessages) : base(acceptedMessages)
    {
        OnlyWithKey = onlyWithKey;
    }
    
    public TelegramQueryStateAttribute() {}

    public override bool CanProcessMessage(Message message, Type targetStateType)
    {
        if (message.Content is not QueryMessageContent query) return base.CanProcessMessage(message, targetStateType);

        if (!OnlyWithKey) return true;

        if (Messages.Any(m => query.Text.StartsWith(m))) return true;
        return false;
    }
}