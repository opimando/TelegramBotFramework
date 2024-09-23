#region Copyright

/*
 * File: KeyboardButton.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public record KeyboardButton(string Title) : Button
{
    public override string ToString()
    {
        return Title;
    }
}