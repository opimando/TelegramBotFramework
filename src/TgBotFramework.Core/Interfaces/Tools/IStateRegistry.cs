#region Copyright

/*
 * File: IStateRegistry.cs
 * Author: denisosipenko
 * Created: 2023-10-14
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Reflection;

namespace TgBotFramework.Core;

public interface IStateRegistry
{
    void Add(Type[] states);
    void Add(Assembly[] assembliesWithStates);
    List<TelegramStateInfo> GetRegisteredStates();
}