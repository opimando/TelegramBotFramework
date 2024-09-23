#region Copyright

/*
 * File: IButtonStructure.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Модель коллекции кнопок в сообщении
/// </summary>
public interface IButtonStructure
{
    int ButtonsCount { get; }
}