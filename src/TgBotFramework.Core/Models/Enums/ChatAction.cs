#region Copyright

/*
 * File: ChatAction.cs
 * Author: denisosipenko
 * Created: 2024-11-18
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public enum ChatAction
{
    Typing = 1,
    UploadPhoto,
    RecordVideo,
    UploadVideo,
    RecordVoice,
    UploadVoice,
    UploadDocument,
    FindLocation,
    RecordVideoNote,
    UploadVideoNote,
    ChooseSticker
}