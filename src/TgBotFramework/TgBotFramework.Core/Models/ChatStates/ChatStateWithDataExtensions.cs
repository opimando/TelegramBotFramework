#region Copyright

/*
 * File: ChatStateWithDataExtensions.cs
 * Author: denisosipenko
 * Created: 2023-09-07
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Reflection;

namespace TgBotFramework.Core;

public static class ChatStateWithDataExtensions
{
    /// <summary>
    /// Проверяет является ли экземпляр <see cref="IChatState"/> реализацией <see cref="IChatStateWithData" />
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static bool IsDataState(this IChatState? state)
    {
        if (state == null) return false;

        Type withDataType = typeof(IChatStateWithData<>);
        Type stateType = state.GetType();

        return stateType.GetInterfaces().Any(intrf => intrf.Name == withDataType.Name);
    }

    /// <summary>
    /// Получить значение наследника <see cref="StateArgument"/> от конкретной реализации <see cref="IChatStateWithData" />.
    /// Если реализация <see cref="IChatState"/> не является наследником <see cref="IChatStateWithData" /> то вернётся просто null
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static StateArgument? GetArgumentByReflectionIfExist(this IChatState? state)
    {
        if (state == null) return null;
        if (!state.IsDataState()) return null;

        MethodInfo? methodInfo = state.GetType().GetMethod("GetData");

        return (StateArgument?) methodInfo!.Invoke(state, null);
    }

    /// <summary>
    /// Устаналивает значение наследника <see cref="StateArgument"/> конкретной реализации <see cref="IChatStateWithData"/>
    /// </summary>
    /// <param name="state"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    public static Task SetArgumentByReflection(this IChatState? state, StateArgument argument)
    {
        if (state == null) return Task.CompletedTask;

        MethodInfo? methodInfo = state.GetType().GetMethod("SetData");

        return Task.Run(() => methodInfo!.Invoke(state, new object?[] {argument}));
    }
}