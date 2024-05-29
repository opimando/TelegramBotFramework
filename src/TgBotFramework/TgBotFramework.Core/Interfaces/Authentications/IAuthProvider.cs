#region Copyright

/*
 * File: IAuthProvider.cs
 * Author: denisosipenko
 * Created: 2023-08-12
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Провайдер доступа пользователя к боту
/// </summary>
public interface IAuthProvider
{
    Task<bool> HasAccess(User user);
    Task<string> GetAccessDeniedMessage();
}