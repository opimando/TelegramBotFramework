#region Copyright

/*
 * File: ContentExtensions.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgAction = Telegram.Bot.Types.Enums.ChatAction;

namespace TgBotFramework.Core;

internal static class ContentExtensions
{
    public static async Task<MessageId> Send(
        this BaseFileContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        return content switch
        {
            VoiceContent voice => await voice.Send(client, chatId, sendInfo, markup, replyTo),
            AudioContent audio => await audio.Send(client, chatId, sendInfo, markup, replyTo),
            DocumentContent doc => await doc.Send(client, chatId, sendInfo, markup, replyTo),
            VideoNoteContent note => await note.Send(client, chatId, sendInfo, markup, replyTo),
            VideoContent video => await video.Send(client, chatId, sendInfo, markup, replyTo),
            ImageContent image => await image.Send(client, chatId, sendInfo, markup, replyTo),
            _ => throw new ArgumentOutOfRangeException(nameof(content), "Не поддерживаемый тип контента")
        };
    }

    public static async Task<MessageId> Send(
        this AudioContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendAudio(
            chatId.Id,
            content.Data == null
                ? InputFile.FromFileId(content.FileId)
                : InputFile.FromStream(content.Data, content.FileName),
            content.Caption,
            replyParameters: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this DocumentContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendDocument(
            chatId.Id,
            content.Data == null
                ? InputFile.FromFileId(content.FileId)
                : InputFile.FromStream(content.Data, content.FileName),
            content.Caption,
            replyParameters: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this ImageContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        InputFile data;
        if (content.Data == null)
        {
            data = InputFile.FromFileId(content.FileId);
        }
        else
        {
            content.Data.Position = 0;
            data = InputFile.FromStream(content.Data, content.FileName);
        }

        Telegram.Bot.Types.Message ret = await client.SendPhoto(
            chatId.Id,
            data,
            content.Caption,
            replyParameters: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this ContactContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendContact(
            chatId.Id,
            content.PhoneNumber,
            content.FirstName,
            content.LastName,
            replyParameters: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this ImageGroupContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        MessageId? replyTo)
    {
        List<IAlbumInputMedia> photos = new();

        List<string> fileNames = new();
        foreach (ImageContent img in content.Images)
        {
            InputFile file;
            if (img.Data != null)
            {
                string fileName = img.FileName ?? string.Empty;
                if (fileNames.Contains(fileName)) fileName = Guid.NewGuid().ToString();
                fileNames.Add(fileName);
                file = InputFile.FromStream(img.Data, fileName);
            }
            else
            {
                file = InputFile.FromFileId(img.FileId);
            }

            var photo = new InputMediaPhoto(file);
            if (photos.Count == 0 && content.Caption != null) photo.Caption = content.Caption;

            photos.Add(photo);
        }

        var ret = await client.SendMediaGroup(chatId.Id,
            photos,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            replyParameters: replyTo?.Id
        );
        return ret.First().MessageId;
    }

    public static async Task<MessageId> Send(
        this LocationContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        int? livePeriod = null;
        if (content.WillBeOnline != TimeSpan.Zero)
            livePeriod = (int) content.WillBeOnline.TotalSeconds;

        Telegram.Bot.Types.Message ret = await client.SendLocation(
            chatId.Id,
            content.Latitude,
            content.Longitude,
            replyTo?.Id,
            markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            livePeriod: livePeriod
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(this ChatActionContent content, ITelegramBotClient client, ChatId chatId)
    {
        TgAction tgAction = content.Action switch
        {
            ChatAction.UploadPhoto => TgAction.UploadPhoto,
            ChatAction.RecordVideo => TgAction.UploadPhoto,
            ChatAction.UploadVideo => TgAction.UploadPhoto,
            ChatAction.RecordVoice => TgAction.UploadPhoto,
            ChatAction.UploadVoice => TgAction.UploadPhoto,
            ChatAction.UploadDocument => TgAction.UploadPhoto,
            ChatAction.FindLocation => TgAction.UploadPhoto,
            ChatAction.RecordVideoNote => TgAction.UploadPhoto,
            ChatAction.UploadVideoNote => TgAction.UploadPhoto,
            ChatAction.ChooseSticker => TgAction.ChooseSticker,
            _ => TgAction.Typing
        };

        await client.SendChatAction(
            chatId.Id,
            tgAction
        );
        return MessageId.NotExistId;
    }

    public static async Task<MessageId> Send(this TextContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo
    )
    {
        string messageText = GetMessageText(content.Content);
        Telegram.Bot.Types.Message ret = await client.SendMessage(
            chatId.Id,
            messageText,
            replyParameters: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this VideoContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        InputFile file;
        if (content.Data != null)
            file = InputFile.FromStream(content.Data, content.FileName ?? Guid.NewGuid().ToString());
        else
            file = InputFile.FromFileId(content.FileId);

        Telegram.Bot.Types.Message ret = await client.SendVideo(
            chatId.Id,
            file,
            replyParameters: replyTo?.Id,
            replyMarkup: markup,
            caption: content.Caption,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this VideoNoteContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        InputFile file;
        if (content.Data != null)
            file = InputFile.FromStream(content.Data, content.FileName ?? Guid.NewGuid().ToString());
        else
            file = InputFile.FromFileId(content.FileId);

        Telegram.Bot.Types.Message ret = await client.SendVideoNote(
            chatId.Id,
            file,
            replyTo?.Id,
            markup,
            (int?) (content.Duration?.TotalSeconds ?? null),
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this VoiceContent content,
        ITelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendVoice(
            chatId.Id,
            content.Data == null
                ? InputFile.FromFileId(content.FileId)
                : InputFile.FromStream(content.Data, content.FileName),
            replyParameters: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static string GetMessageText(this string text)
    {
        return text.Length > MaxMessageTextLength ? text[..(MaxMessageTextLength - 1)] : text;
    }

    private const int MaxMessageTextLength = 4095;
}