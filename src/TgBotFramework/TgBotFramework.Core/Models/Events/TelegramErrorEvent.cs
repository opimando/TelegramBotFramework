#region Copyright

/*
 * File: TelegramErrorEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class TelegramErrorEvent : ErrorEvent
{
    public TelegramErrorEvent(Exception exception) : base(exception)
    {
    }

    public TelegramErrorEvent(string description) : base(description)
    {
    }

    public TelegramErrorEvent(Exception exception, string description) : base(exception, description)
    {
    }

    public override string ToString()
    {
        return $"Поймали ошибку от телеграма: {Description ?? Exception?.Message}";
    }
}