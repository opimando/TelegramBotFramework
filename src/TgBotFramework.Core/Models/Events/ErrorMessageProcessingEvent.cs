#region Copyright

/*
 * File: ErrorMessageProcessingEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ErrorMessageProcessingEvent : ErrorEvent
{
    public ErrorMessageProcessingEvent(Exception exception, Message message) : base(exception,
        $"Ошибка обработки сообщения {message}")
    {
    }
}