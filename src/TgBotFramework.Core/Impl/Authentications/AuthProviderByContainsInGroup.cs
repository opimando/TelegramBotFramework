#region Copyright

/*
 * File: AuthProviderByContainsInGroup.cs
 * Author: denisosipenko
 * Created: 2023-09-03
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Проверка доступности по нахождению в указанной (в конфигурации) группе
/// </summary>
public class AuthProviderByContainsInGroup : IAuthProvider
{
    private readonly IGroupManager _groupManager;
    private readonly ChatId _groupId;

    public AuthProviderByContainsInGroup(IGroupManager groupManager, ChatId groupId)
    {
        _groupManager = groupManager;
        _groupId = groupId;
    }

    /// <inheritdoc />
    public Task<bool> HasAccess(User user)
    {
        return _groupManager.IsMemberInGroup(_groupId, user.Id);
    }

    /// <inheritdoc />
    public Task<string> GetAccessDeniedMessage()
    {
        return Task.FromResult("Нет доступа");
    }
}