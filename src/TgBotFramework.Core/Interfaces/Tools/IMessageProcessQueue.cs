#region Copyright

/*
 * File: IMessageProcessQueue.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Очереди сообщений, которые необходимо обработать.
/// появилась т.к. если пользователь одновременно нажмёт две команды и первая из них будет выполняться долго,
/// то без очереди состояния могут перепутаться.
/// Очередь гарантирует что команды пользователя будут обрабатываться в порядке их отправки боту.
/// </summary>
public interface IMessageProcessQueue
{
    void Enqueue(ChatId chatId, Message newMessage);
}