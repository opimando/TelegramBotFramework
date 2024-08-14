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
        IEventBus eventsBus
    ) : base(
        stateFactory, eventsBus)
    {
        _stateRepository = stateRepository;
    }

    public override Task SaveState(ChatId chatId, IChatState? newState)
    {
        StateArgument?[] arg = newState.GetArgumentByReflectionIfExist();

        var state = new ChatStateEntity
        {
            ChatId = chatId.ToString(),
            SessionId = newState?.SessionId,
            Argument = arg.Any() ? arg : null,
            Type = newState?.GetType()
        };

        return _stateRepository.AddOrUpdate(state);
    }

    public override async Task<IChatState?> GetChatState(ChatId chatId)
    {
        ChatStateEntity? dbState = await _stateRepository.Get(chatId.Id.ToString());
        if (dbState?.Type == null) return null;
        List<StateArgument?> arguments = new();

        if (dbState.Argument != null)
            switch (dbState.Argument)
            {
                case StateArgument stArg:
                    arguments.Add(stArg);
                    break;
                case StateArgument?[] list:
                    arguments.AddRange(list);
                    break;
            }

        IChatState state = await StateFactory.CreateState(dbState.Type, arguments.ToArray());
        state.SessionId = dbState.SessionId ?? Guid.NewGuid();
        if (arguments.Any() && state.IsDataState())
            await state.SetArgumentByReflection(arguments.ToArray());

        return state;
    }
}