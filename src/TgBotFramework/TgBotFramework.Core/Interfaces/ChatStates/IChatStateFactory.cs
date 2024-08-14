#region Copyright

/*
 * File: IChatStateFactory.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Фабрика обработчиков.
/// Нужна т.к. каждый обработчик может пораждаться в любом другом обработчике,
/// при этом зависимости берутся из DI
/// </summary>
public interface IChatStateFactory
{
    Task<T> CreateState<T>(params StateArgument?[] arguments) where T : IChatState;
    Task<IChatState> CreateState(Type stateType, params StateArgument?[] arguments);
}