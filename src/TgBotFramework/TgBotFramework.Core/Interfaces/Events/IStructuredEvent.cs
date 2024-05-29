#region Copyright

/*
 * File: IStructuredEvent.cs
 * Author: denisosipenko
 * Created: 2024-01-12
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public interface IStructuredEvent
{
    string Template { get; }
    object[] Items { get; }
}