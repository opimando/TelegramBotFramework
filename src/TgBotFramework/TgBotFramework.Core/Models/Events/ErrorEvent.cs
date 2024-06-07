#region Copyright

/*
 * File: ErrorEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ErrorEvent : BaseEvent, IStructuredEvent
{
    public ErrorEvent(Exception exception)
    {
        Exception = exception;
    }

    public ErrorEvent(string description)
    {
        Description = description;
    }

    public ErrorEvent(Exception exception, string description) : this(exception)
    {
        Description = description;
    }

    public Exception? Exception { get; }
    public string? Description { get; }

    public override string ToString()
    {
        return $"Произошла ошибка: {Description ?? Exception?.Message}";
    }

    public virtual LogLevel Level => LogLevel.Error;
    public virtual string Template => $"Произошла ошибка {Exception}{Description}";
    public virtual object?[] Items => new object?[] {Exception, Description};
}