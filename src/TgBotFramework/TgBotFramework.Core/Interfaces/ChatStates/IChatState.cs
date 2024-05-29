#region Copyright

/*
 * File: IChatState.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Состояние диалога, умеющее обрабатывать сообщения
/// </summary>
public interface IChatState
{
    /// <summary>
    /// Идентификатор контекста обработки.
    /// Каждый новый экземпляр создаёт новый идентификатор
    /// </summary>
    Guid SessionId { get; set; }
    
    /// <summary>
    /// Обработать полученное сообщение и вернуть новое состояние диалога
    /// </summary>
    /// <param name="receivedMessage">Сообщение из чата</param>
    /// <param name="messenger">Мессенджер для ответа</param>
    /// <returns></returns>
    Task<IChatState?> ProcessMessage(Message receivedMessage, IMessenger messenger);

    /// <summary>
    /// Вызывается при инициализации состояния
    /// </summary>
    /// <param name="messenger">Мессенджер для отправки сообщений</param>
    /// <param name="chatId">Идентификатор чата</param>
    /// <returns></returns>
    Task OnStateStart(IMessenger messenger, ChatId chatId);

    /// <summary>
    /// Вызывается перед тем как состояние сменится на другое
    /// </summary>
    /// <param name="messenger">Мессенджер для ответа</param>
    /// <param name="chatId">Идентификатор чата</param>
    /// <returns></returns>
    Task OnStateExit(IMessenger messenger, ChatId chatId);

    /// <summary>
    /// Поведение при определении следующего состояния
    /// </summary>
    StatePriority Priority { get; }
}