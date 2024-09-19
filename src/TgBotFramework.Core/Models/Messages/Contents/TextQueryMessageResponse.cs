#region Copyright

/*
 * File: TextQueryMessageResponse.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Текстовый ответ на InlineQuery
/// </summary>
public class TextQueryMessageResponse : QueryMessageResponse
{
    public string Text { get; }

    public TextQueryMessageResponse(string title, string fullText, ParseMode parseMode = ParseMode.Markdown) : base(
        title, parseMode)
    {
        Text = fullText;
    }

    public override bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Text) || base.IsEmpty();
    }

    public override string ToString()
    {
        return $"*Текстовый ответ на запрос*: {Text}";
    }
}