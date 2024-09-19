#region Copyright

/*
 * File: SingleUserAuthProvider.cs
 * Author: denisosipenko
 * Created: 2023-09-12
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Только один единственный пользователь имеет доступ
/// </summary>
public class SingleUserAuthProvider : IAuthProvider
{
    public UserId UserId { get; }

    public SingleUserAuthProvider(UserId userId)
    {
        UserId = userId;
    }

    public Task<bool> HasAccess(User user)
    {
        return Task.FromResult(UserId.Equals(user.Id));
    }

    public Task<string> GetAccessDeniedMessage()
    {
        return Task.FromResult("Нет доступа");
    }
}