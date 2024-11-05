#region Copyright

/*
 * File: BaseFileContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public abstract class BaseFileContent : IMessageContent, IDisposable
{
    public BaseFileContent(string fileId, string uniqueFileId, string? fileName)
    {
        FileId = fileId;
        FileName = fileName;
        UniqueFileId = uniqueFileId;
    }

    public BaseFileContent(Stream fileData, string fileName)
    {
        Data = new MemoryStream();
        fileData.CopyTo(Data);
        Data.Seek(0, SeekOrigin.Begin);
        FileName = fileName;
    }

    public MemoryStream? Data { get; }
    public string FileId { get; } = string.Empty;
    public string? FileName { get; }
    public string UniqueFileId { get; } = string.Empty;

    public bool IsEmpty()
    {
        return false;
    }
    //TODO: download method?

    public void Dispose()
    {
        if (Data != null) Data.Dispose();
    }
}