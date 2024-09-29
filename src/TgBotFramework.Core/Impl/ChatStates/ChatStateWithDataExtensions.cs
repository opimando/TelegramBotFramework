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
    public static StateArgument?[] GetArgumentByReflectionIfExist(this IChatState? state)
    {
        if (state == null) return Array.Empty<StateArgument?>();
        if (!state.IsDataState()) return Array.Empty<StateArgument?>();

        List<MethodInfo> methodInfo = state.GetType().GetMethods()
            .Where(m =>
                m.Name == "GetData"
                && m.ReturnParameter.ParameterType.IsAssignableTo(typeof(StateArgument))
            )
            .ToList();

        IEnumerable<StateArgument?> args = methodInfo.Select(s => (StateArgument?) s.Invoke(state, null));

        return args.ToArray();
    }

    /// <summary>
    /// Устаналивает значение наследника <see cref="StateArgument"/> конкретной реализации <see cref="IChatStateWithData"/>
    /// </summary>
    /// <param name="state"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static Task SetArgumentByReflection(this IChatState? state, StateArgument?[] arguments)
    {
        if (state == null) return Task.CompletedTask;

        List<MethodInfo> methodsInfo = GetInterfacesMethods(state.GetType());

        List<(StateArgument? Argument, MethodInfo? Method)> argumentWithMethods = arguments
            .Where(s => s != null)
            .Select(a =>
            {
                MethodInfo? method = methodsInfo.FirstOrDefault(m =>
                    a!.GetType().IsAssignableTo(m.GetParameters()[0].ParameterType)
                );
                return (a, method);
            }).ToList();

        IEnumerable<Task> tasks = argumentWithMethods
            .Where(s => s.Method != null && s.Argument != null)
            .Select(p => Task.Run(() => { p.Method!.Invoke(state, new object?[] {p.Argument}); }));
        return Task.WhenAll(tasks);
    }

    private static List<MethodInfo> GetInterfacesMethods(Type stateType)
    {
        List<MethodInfo> ret = new();
        try
        {
            Type dataInterface = typeof(IChatStateWithData<>);

            Type[] implementedInterfaces = stateType.GetInterfaces();

            foreach (Type implemented in implementedInterfaces)
                if (implemented.IsGenericType && implemented.GetGenericTypeDefinition() == dataInterface)
                {
                    InterfaceMapping map = stateType.GetInterfaceMap(implemented);
                    IEnumerable<MethodInfo> methods = map.InterfaceMethods.Where(m => m.Name == "SetData");
                    ret.AddRange(methods);
                }
        }
        catch (Exception)
        {
            //ignore
        }

        return ret.Distinct().ToList();
    }
}