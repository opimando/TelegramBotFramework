#region Copyright

/*
 * File: TelegramStateAttribute.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Reflection;

namespace TgBotFramework.Core;

/// <summary>
/// Аттрибут, которым помечается класс, реализуюзий <see cref="IChatState"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TelegramStateAttribute : Attribute
{
    public bool NeedIgnore { get; }
    public string[] Messages { get; }
    public bool HasCustomAccessFunction { get; }

    public TelegramStateAttribute(bool needIgnore, bool hasCustomAccessFunction,
        params string[] acceptedMessages) :
        this(hasCustomAccessFunction, acceptedMessages)
    {
        NeedIgnore = needIgnore;
    }

    public TelegramStateAttribute(bool hasCustomAccessFunction, params string[] acceptedMessages) : this(
        acceptedMessages)
    {
        HasCustomAccessFunction = hasCustomAccessFunction;
    }

    public TelegramStateAttribute(params string[] acceptedMessages)
    {
        Messages = acceptedMessages;
    }

    public virtual bool CanProcessMessage(Message message, Type targetStateType)
    {
        if (HasCustomAccessFunction)
        {
            Func<Message, bool> customFunction = GetClassCustomFunction(targetStateType);
            return customFunction(message);
        }

        string textToCompare;
        switch (message.Content)
        {
            case TextContent text:
                textToCompare = text.Content;
                break;
            case QueryMessageContent query:
                textToCompare = query.Text;
                break;
            case CallbackInlineButtonContent callback:
                textToCompare = callback.Data ?? callback.QueryId;
                break;
            default:
                return false;
        }

        return Messages.Any(msg => msg.Equals(textToCompare, StringComparison.OrdinalIgnoreCase));
    }

    private Func<Message, bool> GetClassCustomFunction(Type type)
    {
        MethodInfo? method = type.GetMethods()
            .FirstOrDefault(m => m.GetCustomAttribute<CustomStaticAccessFunctionAttribute>() != null);
        if (method != null) return message => (bool) method.Invoke(null, new object?[] {message})!;
        if (type.BaseType == null) return _ => false;
        return GetClassCustomFunction(type.BaseType);
    }
}