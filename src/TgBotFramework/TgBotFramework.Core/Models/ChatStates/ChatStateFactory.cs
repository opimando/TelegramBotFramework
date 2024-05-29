#region Copyright

/*
 * File: ChatStateFactory.cs
 * Author: denisosipenko
 * Created: 2023-08-11
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ChatStateFactory : IChatStateFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ChatStateFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<T> CreateState<T>(StateArgument? argument = null) where T : IChatState
    {
        var state = _serviceProvider.Create<T>();
        await SetDataIfExist(state, argument);
        return state;
    }

    public async Task<IChatState> CreateState(Type stateType, StateArgument? argument = null)
    {
        var state = _serviceProvider.Create<IChatState>(stateType);
        await SetDataIfExist(state, argument);
        return state;
    }

    private async Task SetDataIfExist(IChatState state, StateArgument? argument = null)
    {
        if (argument == null) return;
        if (!state.IsDataState()) return;

        await state.SetArgumentByReflection(argument);
    }
}