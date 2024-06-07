#region Copyright

/*
 * File: SetBotDescriptionEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class SetBotDescriptionEvent : BaseEvent, IStructuredEvent
{
    public SetBotDescriptionEvent(string newDescription)
    {
        Description = newDescription;
    }

    public string Description { get; }

    public override string ToString()
    {
        return $"Установлено описание бота: {Description}";
    }

    public LogLevel Level => LogLevel.Debug;
    public string Template => "Установлено описание бота: {Description}";
    public object?[] Items => new object?[] {Description};
}