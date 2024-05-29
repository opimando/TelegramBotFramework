#region Copyright

/*
 * File: UserId.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class UserId : ChatId
{
    public UserId(long id) : base(id)
    {
    }
}