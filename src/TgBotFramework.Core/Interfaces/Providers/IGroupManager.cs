#region Copyright

/*
 * File: IGroupManager.cs
 * Author: denisosipenko
 * Created: 2024-04-03
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Управление данными в группе
/// </summary>
public interface IGroupManager
{
    Task<bool> IsMemberInGroup(ChatId chatId, UserId userId);
    Task KickUserFromGroup(ChatId chatId, UserId userId);
}