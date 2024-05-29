#region Copyright

/*
 * File: UserHasNotAccessEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class UserHasNotAccessEvent : BaseEvent, IStructuredEvent
{
    public UserHasNotAccessEvent(User user)
    {
        User = user;
    }

    public User User { get; }

    public string Template => "У пользователя {User} нет доступа к боту";
    public object[] Items => new object[] {User};
}