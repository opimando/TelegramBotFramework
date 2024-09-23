#region Copyright

/*
 * File: StateRegistry.cs
 * Author: denisosipenko
 * Created: 2023-10-14
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Reflection;

namespace TgBotFramework.Core;

public class StateRegistry : IStateRegistry
{
    private readonly List<TelegramStateInfo> _registeredStates = new();

    public void Add(Type[] states)
    {
        IEnumerable<TelegramStateInfo> telegramCommands = GetTelegramCommandsInfo(states);
        _registeredStates.AddRange(telegramCommands);
    }

    public void Add(Assembly[] assembliesWithStates)
    {
        foreach (Assembly assembly in assembliesWithStates)
        {
            IEnumerable<TelegramStateInfo> telegramCommands = GetAssemblyTelegramCommands(assembly);
            _registeredStates.AddRange(telegramCommands);
        }
    }

    public List<TelegramStateInfo> GetRegisteredStates()
    {
        return new List<TelegramStateInfo>(_registeredStates);
    }

    protected virtual IEnumerable<TelegramStateInfo> GetAssemblyTelegramCommands(Assembly assembly)
    {
        return GetTelegramCommandsInfo(assembly.GetTypes());
    }

    protected virtual IEnumerable<TelegramStateInfo> GetTelegramCommandsInfo(IEnumerable<Type> types)
    {
        List<Type> typesList = types.ToList();

        foreach (Type type in typesList)
        {
            List<TelegramStateAttribute> attributes =
                type.GetCustomAttributes<TelegramStateAttribute>(true).ToList();

            if (attributes.Count <= 0)
                continue;

            TelegramStateAttribute attribute = attributes.First();

            if (attribute.NeedIgnore)
                continue;

            yield return new TelegramStateInfo(type, attributes.First());
        }
    }
}