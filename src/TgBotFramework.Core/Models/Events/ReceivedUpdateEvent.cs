#region Copyright

/*
 * File: ReceivedUpdateEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ReceivedUpdateEvent : BaseEvent, IStructuredEvent
{
    public ReceivedUpdateEvent(string meta)
    {
        Meta = meta;
    }

    public string Meta { get; }

    public override string ToString()
    {
        return $"Получено уведомление от телеграма {Meta}";
    }

    public LogLevel Level => LogLevel.Debug;
    public string Template => "Получено уведомление от телеграма {Meta}";
    public object?[] Items => new object?[] {Meta};
}