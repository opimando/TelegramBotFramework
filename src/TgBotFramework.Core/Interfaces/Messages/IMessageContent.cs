#region Copyright

/*
 * File: IMessageContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Содержимое сообщения
/// </summary>
public interface IMessageContent
{
    bool IsEmpty();
}