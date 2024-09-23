#region Copyright

/*
 * File: TelegramStateInfo.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public record TelegramStateInfo(Type StateType, TelegramStateAttribute StateAttribute)
{
    public override string ToString()
    {
        return StateType.Name;
    }
}