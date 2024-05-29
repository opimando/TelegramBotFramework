#region Copyright

/*
 * File: VoiceContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class VoiceContent : AudioContent
{
    public VoiceContent(string fileId, string uniqueFileId, int durationSec) : base(fileId, null, uniqueFileId,
        durationSec, null)
    {
    }

    public VoiceContent(Stream fileData, string fileName) : base(fileData, fileName)
    {
    }

    public override string ToString()
    {
        return $"*Голосовое сообщение*: {UniqueFileId}";
    }
}