#region Copyright

/*
 * File: InlineType.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public enum InlineType
{
    Basic,
    Url,
    Switch,
    SwitchCurrentChat,
    SwitchChooseChat,
    Pay,
    WebApp
}