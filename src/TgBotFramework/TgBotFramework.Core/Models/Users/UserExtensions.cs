#region Copyright

/*
 * File: UserExtensions.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

internal static class UserExtensions
{
    private const string UNKNOWN_NAME = "*unknown*";

    public static User GetLocal(this Telegram.Bot.Types.User tgUser)
    {
        string username = string.IsNullOrWhiteSpace(tgUser.Username) ? UNKNOWN_NAME : tgUser.Username;
        string friendlyName = string.IsNullOrWhiteSpace(tgUser.Username) ? UNKNOWN_NAME : tgUser.Username;

        return new User(new UserId(tgUser.Id), username, friendlyName);
    }
}