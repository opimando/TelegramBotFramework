#region Copyright

/*
 * File: BaseStateStore.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public abstract class BaseStateStore : IChatStateStore
{
    protected readonly IChatStateFactory StateFactory;
    protected readonly IEventBus EventsBus;
    protected List<TelegramStateInfo> SupportedStates = new();

    public BaseStateStore(IChatStateFactory stateFactory, IEventBus eventsBus)
    {
        StateFactory = stateFactory;
        EventsBus = eventsBus;
    }

    #region Abstract methods

    public abstract Task SaveState(ChatId chatId, IChatState? newState);

    public abstract Task<IChatState?> GetChatState(ChatId chatId);

    #endregion Abstract methods

    #region Public methods

    public virtual async Task<IChatState?> GetNewHandlerForRequest(Message receivedMessage)
    {
        if (receivedMessage == default)
            throw new ArgumentNullException(nameof(receivedMessage));

        List<IChatState> supportedStates = await GetSupportedForMessageStates(receivedMessage);

        IChatState? nextState = supportedStates.Any()
            ? supportedStates.MaxBy(s => s.Priority)
            : null;

        return nextState;
    }

    public virtual async Task PushState(ChatId chatId, IChatState? state)
    {
        IChatState? currentState = await GetChatState(chatId);
        await NotifyReplacedStates(chatId, currentState, state);
        await SaveState(chatId, state);
    }

    public virtual async Task NotifyReplacedStates(ChatId chatId, IChatState? oldState, IChatState? newState)
    {
        //если у нас не было стейта, то нечему завершаться. Если у нас нет следующего состояния, но есть текущее, то не за чем завершать старое
        if (oldState != null)
            try
            {
                await oldState.OnStateExit(chatId);
            }
            catch (Exception ex)
            {
                EventsBus.Publish(new ErrorEvent(ex,
                    $"Ошибка при обработке завершения состояния {oldState} чата {chatId}"));
            }

        if (newState != null)
            try
            {
                await newState.OnStateStart(chatId);
            }
            catch (Exception ex)
            {
                EventsBus.Publish(new ErrorEvent(ex,
                    $"Ошибка при обработке запуска состояния {newState} чата {chatId}"));
            }

        EventsBus.Publish(new UserStateChangedEvent(chatId, oldState, newState));
    }

    public virtual void SetStates(List<TelegramStateInfo> states)
    {
        SupportedStates = states;
    }

    #endregion Public methods

    #region Protected methods

    protected virtual async Task<List<IChatState>> GetSupportedForMessageStates(Message receivedMessage)
    {
        List<IChatState> res = new();

        foreach (TelegramStateInfo info in SupportedStates)
            try
            {
                if (info.StateAttribute.NeedIgnore) continue;
                if (info.StateAttribute.CanProcessMessage(receivedMessage, info.StateType))
                    res.Add(await StateFactory.CreateState(info.StateType));
            }
            catch (Exception ex)
            {
                EventsBus.Publish(new ErrorEvent(ex,
                    $"Ошибка при попытке создать экземпляр состояния {info.StateType} на сообщение {receivedMessage}"));
            }

        return res;
    }

    #endregion Protected methods
}