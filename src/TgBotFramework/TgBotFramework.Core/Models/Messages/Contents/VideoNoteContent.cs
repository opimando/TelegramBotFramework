#region Copyright

/*
 * File: VideoNoteContent.cs
 * Author: denisosipenko
 * Created: 2024-07-03
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class VideoNoteContent : VideoContent
{
    public VideoNoteContent(string fileId, string uniqueFileId, long? fileSize, int durationSec, ImageContent? preview)
        : base(fileId, uniqueFileId, fileSize, 0, 0, durationSec, preview)
    {
    }

    public VideoNoteContent(Stream fileData, string fileName) : base(fileData, fileName)
    {
    }

    public override string ToString()
    {
        return $"*Видео кружок*: {UniqueFileId}";
    }
}