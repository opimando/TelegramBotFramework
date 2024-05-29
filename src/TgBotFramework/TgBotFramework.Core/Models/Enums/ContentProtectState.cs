#region Copyright

/*
 * File: ContentProtectState.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public enum ContentProtectState
{
    Unknown,
    Protected,
    Public
}

public static class ContentProtectExtensions
{
    public static bool IsContentProtected(this ContentProtectState state)
    {
        if (state is ContentProtectState.Unknown) return false;

        return state is ContentProtectState.Protected;
    }
}