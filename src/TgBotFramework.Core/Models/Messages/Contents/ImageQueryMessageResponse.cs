#region Copyright

/*
 * File: ImageQueryMessageResponse.cs
 * Author: denisosipenko
 * Created: 2025-03-26
 * Copyright © 2025 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ImageQueryMessageResponse : QueryMessageResponse
{
    /// <summary>
    /// 5MB max, jpeg format
    /// </summary>
    public string PhotoUrl { get; }

    public string ThumbnailUrl => PhotoUrl;
    /// <summary>
    /// не работает, как и title
    /// </summary>
    public string? ResultDescription { get; }
    public string? ImageCaptionInMessage { get; }
    public int? Width { get; }
    public int? Height { get; }

    public ImageQueryMessageResponse(
        string photoUrl,
        string resultTitle,
        string? resultDescription = null,
        string? imageCaptionInMessage = null,
        int? width = null,
        int? height = null,
        ParseMode parseMode = ParseMode.Markdown
        ) : base(resultTitle, parseMode)
    {
        PhotoUrl = photoUrl;
        ResultDescription = resultDescription;
        ImageCaptionInMessage = imageCaptionInMessage;
        Width = width;
        Height = height;
    }
    
    public override string ToString()
    {
        return $"*Ответ-картинка на запрос*: {PhotoUrl} {Title}";
    }
}