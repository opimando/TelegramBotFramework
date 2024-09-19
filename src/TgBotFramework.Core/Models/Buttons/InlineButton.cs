#region Copyright

/*
 * File: InlineButton.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public record InlineButton : KeyboardButton
{
    public string Data { get; }
    public InlineType Type { get; }

    public InlineButton(string title, string? data = null, InlineType type = InlineType.Basic) : base(title)
    {
        Data = data ?? title;
        Type = type;
    }
}