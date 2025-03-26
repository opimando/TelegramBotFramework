#region Copyright

/*
 * File: ImageListItemQueryMessageResponse.cs
 * Author: denisosipenko
 * Created: 2025-03-26
 * Copyright © 2025 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ImageListItemQueryMessageResponse : QueryMessageResponse
{
    /// <summary>
    /// Сразу редиректит на урл без отправки ответа в чат
    /// </summary>
    public string? Url { get; }
    public string? Description { get; }
    public string? Thumbnail { get; }

    public ImageListItemQueryMessageResponse(
        string title,
        string? url = null,
        string? description = null,
        string? thumbnail = null
        ) : base(title)
    {
        Url = url;
        Description = description;
        Thumbnail = thumbnail;
    }
}