#region Copyright

/*
 * File: User.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class User
{
    public User(UserId id, string userName, string friendlyName)
    {
        Id = id;
        UserName = userName;
        FriendlyName = friendlyName;
    }

    public UserId Id { get; }
    public string UserName { get; }
    public string FriendlyName { get; }

    public override string ToString()
    {
        return $"{UserName} ({Id})";
    }
}