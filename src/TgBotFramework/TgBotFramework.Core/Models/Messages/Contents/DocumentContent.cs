#region Copyright

/*
 * File: DocumentContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class DocumentContent : BaseFileContent
{
    public DocumentContent(string fileId, string uniqueFileId, string? fileName, long? fileSize) : base(fileId,
        uniqueFileId, fileName)
    {
        FileSize = fileSize;
    }

    public DocumentContent(Stream fileData, string fileName) : base(fileData, fileName)
    {
    }

    public long? FileSize { get; }

    public override string ToString()
    {
        return $"*Документ*: {UniqueFileId}";
    }
}