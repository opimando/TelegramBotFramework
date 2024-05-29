#region Copyright

/*
 * File: VideoContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class VideoContent : ImageContent
{
    public ImageContent? Preview { get; }
    public TimeSpan Duration { get; }

    public VideoContent(string fileId, string uniqueFileId, long? fileSize, int width, int height, int durationSec,
        ImageContent? preview) : base(fileId, uniqueFileId, fileSize, width, height)
    {
        Preview = preview;
        Duration = TimeSpan.FromSeconds(durationSec);
    }

    public VideoContent(Stream fileData, string fileName) : base(fileData, fileName)
    {
    }

    public override string ToString()
    {
        return $"*Видео файл*: {UniqueFileId}";
    }
}