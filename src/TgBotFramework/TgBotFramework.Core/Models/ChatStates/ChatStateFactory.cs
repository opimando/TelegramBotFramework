#region Copyright

/*
 * File: ChatStateFactory.cs
 * Author: denisosipenko
 * Created: 2023-08-11
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Reflection;

namespace TgBotFramework.Core;

public class ChatStateFactory : IChatStateFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ChatStateFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<T> CreateState<T>(params StateArgument?[] arguments) where T : IChatState
    {
        var state = _serviceProvider.Create<T>();
        SetDefaultServicesToState(state);
        await SetDataIfExist(state, arguments);
        return state;
    }

    public async Task<IChatState> CreateState(Type stateType, params StateArgument?[] arguments)
    {
        var state = _serviceProvider.Create<IChatState>(stateType);
        SetDefaultServicesToState(state);
        await SetDataIfExist(state, arguments);
        return state;
    }

    private async Task SetDataIfExist(IChatState state, StateArgument?[] arguments)
    {
        if (arguments.Length == 0) return;
        if (!state.IsDataState()) return;

        await state.SetArgumentByReflection(arguments);
    }

    private void SetDefaultServicesToState(IChatState? state)
    {
        if (state == null) return;

        Type type = state.GetType();
        List<PropertyInfo> props = type.GetProperties()
            .Where(s =>
                (s.PropertyType == typeof(IMessenger) || s.PropertyType == typeof(IEventBus))
                && s.SetMethod != null
            )
            .ToList();

        foreach (PropertyInfo prop in props)
        {
            object? service = _serviceProvider.GetService(prop.PropertyType);

            if (service == null)
                continue;

            prop.SetValue(state, service);
        }
    }
}