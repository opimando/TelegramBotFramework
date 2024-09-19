#region Copyright

/*
 * File: DocumentContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class DocumentContent : CaptionContent
{
    public DocumentContent(string fileId, string uniqueFileId, string? fileName, long? fileSize,
        string? caption = null) : base(fileId,
        uniqueFileId, fileName, caption)
    {
        FileSize = fileSize;
    }

    public DocumentContent(Stream fileData, string fileName, string? caption = null) : base(fileData, fileName, caption)
    {
    }

    public long? FileSize { get; }

    public override string ToString()
    {
        return $"*Документ*: {UniqueFileId}";
    }
}