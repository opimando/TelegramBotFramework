#region Copyright

/*
 * File: CaptionContent.cs
 * Author: denisosipenko
 * Created: 2024-06-10
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class CaptionContent : BaseFileContent
{
    public string? Caption { get; }

    public CaptionContent(string fileId, string uniqueFileId, string? fileName, string? caption = null) : base(fileId,
        uniqueFileId, fileName)
    {
        Caption = caption;
    }

    public CaptionContent(Stream fileData, string fileName, string? caption = null) : base(fileData, fileName)
    {
        Caption = caption;
    }
}