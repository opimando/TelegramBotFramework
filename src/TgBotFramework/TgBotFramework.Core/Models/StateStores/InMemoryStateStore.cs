#region Copyright

/*
 * File: InMemoryStateStore.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Collections.Concurrent;

namespace TgBotFramework.Core;

public class InMemoryStateStore : BaseStateStore
{
    private readonly ConcurrentDictionary<ChatId, IChatState?> _userStates = new();

    public InMemoryStateStore(IChatStateFactory stateFactory, IEventBus eventsBus) : base(stateFactory, eventsBus)
    {
    }

    #region Implementation of BaseStateStore

    public override Task SaveState(ChatId chatId, IChatState? newState)
    {
        _userStates.AddOrUpdate(
            chatId,
            key => newState,
            (key, old) => newState
        );
        return Task.CompletedTask;
    }

    public override Task<IChatState?> GetChatState(ChatId chatId)
    {
        _userStates.TryGetValue(chatId, out IChatState? state);
        return Task.FromResult(state);
    }

    #endregion Implementation of BaseStateStore
}