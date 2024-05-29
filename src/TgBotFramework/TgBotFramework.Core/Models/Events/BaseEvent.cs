#region Copyright

/*
 * File: BaseEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Базовое событие, возникающее во время работы бота
/// </summary>
public abstract class BaseEvent
{
    public DateTime Created { get; init; } = DateTime.Now;

    public override string? ToString()
    {
        if (this is IStructuredEvent structuredEvent)
            return string.Format(structuredEvent.Template, structuredEvent.Items);

        return base.ToString();
    }
}