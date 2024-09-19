#region Copyright

/*
 * File: TelegramErrorEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class TelegramErrorEvent : ErrorEvent, IStructuredEvent
{
    public TelegramErrorEvent(Exception exception) : base(exception)
    {
    }

    public override string ToString()
    {
        return $"Поймали ошибку от телеграма: {Description ?? Exception?.Message}";
    }

    public override LogLevel Level => LogLevel.Error;
    public override string Template => "Получили ошибку от Telegram {@Exception}";
    public override object[] Items => new object[] {Exception!};
}