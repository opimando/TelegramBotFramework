#region Copyright

/*
 * File: IChatStateStore.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Хранилище всех состояний пользователей
/// </summary>
public interface IChatStateStore
{
    /// <summary>
    /// Вернуть состояние, которое должно обработать сообщение.
    /// Если при этом состояние изменяется, то происходит уведомление об этом
    /// </summary>
    /// <remarks>
    /// Если последний установленный стейт есть и он может обработать сообщение, то вернётся он
    /// Если такого нет, то ищем всевозможные стейты для этого сообщения
    /// </remarks>
    /// <param name="receivedMessage">Пришедшее сообщение от телеграма</param>
    /// <returns>Список стейтов, умеющих обработать сообщение.</returns>
    Task<IChatState?> GetNewHandlerForRequest(Message receivedMessage);

    /// <summary>
    /// Сохранить состояние для диалога
    /// </summary>
    /// <param name="chatId">Идентификатор диалога</param>
    /// <param name="newState">Новое установленное состояние</param>
    /// <returns></returns>
    Task SaveState(ChatId chatId, IChatState? newState);

    /// <summary>
    /// Возвращает текущее состояние в диалоге
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <returns></returns>
    Task<IChatState?> GetChatState(ChatId chatId);

    /// <summary>
    /// Установить состояние без запроса клиента
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <param name="state">Состояние</param>
    /// <returns></returns>
    Task PushState(ChatId chatId, IChatState? state);

    /// <summary>
    /// Уведомить об измении состояния
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="oldState"></param>
    /// <param name="newState"></param>
    /// <returns></returns>
    Task NotifyReplacedStates(ChatId chatId, IChatState? oldState, IChatState? newState);

    void SetStates(List<TelegramStateInfo> states);
}