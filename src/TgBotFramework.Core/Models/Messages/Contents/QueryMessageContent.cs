#region Copyright

/*
 * File: QueryMessageContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// InlineQuery который отправляет пользователь
/// </summary>
public class QueryMessageContent : IMessageContent
{
    public QueryMessageContent(string messageQueryId, string text)
    {
        MessageQueryId = messageQueryId;
        Text = text;
    }

    public string MessageQueryId { get; }
    public string Text { get; }
    public string Offset { get; set; } = string.Empty;

    public bool IsEmpty()
    {
        return false;
    }

    public override string ToString()
    {
        return $"*Содержимое запроса*: QueryId - {MessageQueryId}; Текст - {Text}";
    }
}