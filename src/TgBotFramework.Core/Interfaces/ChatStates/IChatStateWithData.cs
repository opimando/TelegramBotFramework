#region Copyright

/*
 * File: IChatStateWithData.cs
 * Author: denisosipenko
 * Created: 2023-08-13
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Состояние с хранимыми данными.
/// Зависит от контекста, устанавливаемого в StateArgument
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IChatStateWithData<T> : IChatState where T : StateArgument
{
    T GetData();
    Task SetData(T data);
}