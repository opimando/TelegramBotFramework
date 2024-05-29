#region Copyright

/*
 * File: ImageContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ImageContent : BaseFileContent
{
    public ImageContent(string fileId, string uniqueFileId, long? fileSize, int width, int height) : base(fileId,
        uniqueFileId, string.Empty)
    {
        FileSize = fileSize;
        Width = width;
        Height = height;
    }

    public ImageContent(Stream fileData, string fileName) : base(fileData, fileName)
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