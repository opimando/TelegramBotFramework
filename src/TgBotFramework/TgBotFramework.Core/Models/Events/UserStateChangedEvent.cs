#region Copyright

/*
 * File: UserStateChangedEvent.cs
 * Author: denisosipenko
 * Created: 2024-04-30
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class UserStateChangedEvent : BaseEvent, IStructuredEvent
{
    public IChatState? OldState { get; }
    public IChatState? NewState { get; }
    public ChatId ChatId { get; }

    public UserStateChangedEvent(ChatId chatId, IChatState? oldState, IChatState? newState)
    {
        ChatId = chatId;
        OldState = oldState;
        NewState = newState;
    }

    public LogLevel Level => LogLevel.Debug;
    public string Template => "В чате {ChatId} изменено состояние с {@OldState} на {@NewState}";

    public object?[] Items => new object?[] {ChatId, OldState, NewState};
}