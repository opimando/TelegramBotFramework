#region Copyright

/*
 * File: AudioContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class AudioContent : CaptionContent
{
    public AudioContent(string fileId, string? fileName, string uniqueFileId, int durationSec, string? title,
        string? caption = null) : base(
        fileId, uniqueFileId, fileName, caption)
    {
        Title = title;
        Duration = TimeSpan.FromSeconds(durationSec);
    }

    public AudioContent(Stream fileData, string fileName, string? caption = null) : base(fileData, fileName, caption)
    {
    }

    public TimeSpan Duration { get; }
    public string? Title { get; }

    public override string ToString()
    {
        return $"*Аудио файл*: {UniqueFileId}";
    }
}