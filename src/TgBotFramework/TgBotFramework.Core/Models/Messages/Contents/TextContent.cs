#region Copyright

/*
 * File: TextContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class TextContent : IMessageContent
{
    public TextContent(string content)
    {
        Content = content;
    }

    public string Content { get; }

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Content);
    }

    public static implicit operator TextContent(string text)
    {
        return new TextContent(text);
    }

    public override string ToString()
    {
        return $"*Текст*: '{Content}'";
    }
}