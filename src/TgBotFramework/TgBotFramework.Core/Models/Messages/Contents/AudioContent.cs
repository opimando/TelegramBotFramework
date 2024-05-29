#region Copyright

/*
 * File: AudioContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class AudioContent : BaseFileContent
{
    public AudioContent(string fileId, string? fileName, string uniqueFileId, int durationSec, string? title) : base(
        fileId, uniqueFileId, fileName)
    {
        Title = title;
        Duration = TimeSpan.FromSeconds(durationSec);
    }

    public AudioContent(Stream fileData, string fileName) : base(fileData, fileName)
    {
    }

    public TimeSpan Duration { get; }
    public string? Title { get; }

    public override string ToString()
    {
        return $"*Аудио файл*: {UniqueFileId}";
    }
}