#region Copyright

/*
 * File: QueryMessageResponse.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Ответ на inlineQuery (при вводе в поле текста)
/// </summary>
public abstract class QueryMessageResponse : IMessageContent
{
    /// <summary>
    /// Всегда уникальный, как я понял
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();
    public ParseMode ParseMode { get; }

    /// <summary>
    /// Краткий заголовок ответа
    /// </summary>
    public string Title { get; }

    public QueryMessageResponse(string title, ParseMode parseMode = ParseMode.Markdown)
    {
        Title = title;
        ParseMode = parseMode;
    }

    public virtual bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Title);
    }

    public override string ToString()
    {
        return $"*Ответ на запрос*: {Title}";
    }
}