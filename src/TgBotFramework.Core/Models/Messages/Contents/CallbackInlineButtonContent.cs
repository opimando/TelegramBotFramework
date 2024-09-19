#region Copyright

/*
 * File: CallbackQueryMessageContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Ответ который прилетает при нажатии на InlineButton
/// </summary>
public class CallbackInlineButtonContent : IMessageContent
{
    public CallbackInlineButtonContent(string queryId, string? data)
    {
        QueryId = queryId;
        Data = data;
    }

    public string QueryId { get; }
    public string? Data { get; }

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Data);
    }

    public override string ToString()
    {
        return $"Id запроса: {QueryId}, содержимое: {Data}";
    }
}