#region Copyright

/*
 * File: ParseMode.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using TgMode = Telegram.Bot.Types.Enums.ParseMode;

namespace TgBotFramework.Core;

public enum ParseMode
{
    Markdown,
    Html,
    MarkdownV2
}

internal static class ParseModeExtensions
{
    public static TgMode GetParseMode(this ParseMode mode)
    {
        return mode switch
        {
            ParseMode.Html => TgMode.Html,
            ParseMode.MarkdownV2 => TgMode.MarkdownV2,
            _ => TgMode.Markdown
        };
    }
}