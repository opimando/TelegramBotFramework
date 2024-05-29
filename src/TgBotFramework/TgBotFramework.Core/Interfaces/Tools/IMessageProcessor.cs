#region Copyright

/*
 * File: IMessageProcessor.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// При получении сообщения должен найти handler и вызывать обработку
/// </summary>
public interface IMessageProcessor
{
    Task Process(Message message);
}