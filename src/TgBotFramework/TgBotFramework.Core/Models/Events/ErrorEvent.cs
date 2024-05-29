#region Copyright

/*
 * File: ErrorEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ErrorEvent : BaseEvent
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

    public Exception? Exception { get; set; }
    public string? Description { get; set; }

    public override string ToString()
    {
        return $"Произошла ошибка: {Description ?? Exception?.Message}";
    }
}