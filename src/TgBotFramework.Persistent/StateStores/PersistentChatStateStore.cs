#region Copyright

/*
 * File: PersistentChatStateStore.cs
 * Author: denisosipenko
 * Created: 2023-11-15
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using TgBotFramework.Core;

namespace TgBotFramework.Persistent;

public class PersistentChatStateStore : BaseStateStore
{
    private readonly IStateRepository _stateRepository;

    public PersistentChatStateStore(
        IStateRepository stateRepository,
        IChatStateFactory stateFactory,
        IEventBus eventsBus,
        IMessenger messenger
    ) : base(
        stateFactory, eventsBus, messenger)
    {
        _stateRepository = stateRepository;
    }

    public override Task SaveState(ChatId chatId, IChatState? newState)
    {
        StateArgument? arg = newState.GetArgumentByReflectionIfExist();

        var state = new ChatStateEntity
        {
            ChatId = chatId.ToString(),
            SessionId = newState?.SessionId,
            Argument = arg,
            Type = newState?.GetType()
        };

        return _stateRepository.AddOrUpdate(state);
    }

    public override async Task<IChatState?> GetChatState(ChatId chatId)
    {
        ChatStateEntity? dbState = await _stateRepository.Get(chatId.Id.ToString());
        if (dbState?.Type == null) return null;
        var argument = dbState.Argument as StateArgument;

        IChatState state = await StateFactory.CreateState(dbState.Type, argument);
        state.SessionId = dbState.SessionId ?? Guid.NewGuid();
        if (argument != null && state.IsDataState())
            await state.SetArgumentByReflection(argument);

        return state;
    }
}