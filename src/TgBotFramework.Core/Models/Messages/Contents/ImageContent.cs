#region Copyright

/*
 * File: ImageContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ImageContent : CaptionContent
{
    public ImageContent(string fileId, string uniqueFileId, long? fileSize, int width, int height,
        string? caption = null) : base(fileId,
        uniqueFileId, string.Empty, caption)
    {
        FileSize = fileSize;
        Width = width;
        Height = height;
    }

    public ImageContent(Stream fileData, string fileName, string? caption = null) : base(fileData, fileName, caption)
    {
    }

    public long? FileSize { get; }
    public int Width { get; }
    public int Height { get; }

    public override string ToString()
    {
        return $"*Изображение*: {UniqueFileId}";
    }
}