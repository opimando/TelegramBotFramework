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

namespace TgBotFramework.Core;

internal static class ContentExtensions
{
    public static async Task<MessageId> Send(
        this BaseFileContent content,
        TelegramBotClient client,
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
            VideoContent video => await video.Send(client, chatId, sendInfo, markup, replyTo),
            ImageContent image => await image.Send(client, chatId, sendInfo, markup, replyTo),
            _ => throw new ArgumentOutOfRangeException("Не поддерживаемый тип контента")
        };
    }

    public static async Task<MessageId> Send(
        this AudioContent content,
        TelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendAudioAsync(
            chatId.Id,
            content.Data == null
                ? InputFile.FromFileId(content.FileId)
                : InputFile.FromStream(content.Data, content.FileName),
            caption: content.Caption,
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this DocumentContent content,
        TelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendDocumentAsync(
            chatId.Id,
            content.Data == null
                ? InputFile.FromFileId(content.FileId)
                : InputFile.FromStream(content.Data, content.FileName),
            caption: content.Caption,
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this ImageContent content,
        TelegramBotClient client,
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

        Telegram.Bot.Types.Message ret = await client.SendPhotoAsync(
            chatId.Id,
            data,
            caption: content.Caption,
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this ContactContent content,
        TelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendContactAsync(
            chatId.Id,
            content.PhoneNumber,
            content.FirstName,
            lastName: content.LastName,
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this ImageGroupContent content,
        TelegramBotClient client,
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

        Telegram.Bot.Types.Message[] ret = await client.SendMediaGroupAsync(chatId.Id,
            photos,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            replyToMessageId: replyTo?.Id
        );
        return ret.First().MessageId;
    }

    public static async Task<MessageId> Send(
        this LocationContent content,
        TelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        int? livePeriod = null;
        if (content.WillBeOnline != TimeSpan.Zero)
            livePeriod = (int) content.WillBeOnline.TotalSeconds;

        Telegram.Bot.Types.Message ret = await client.SendLocationAsync(
            chatId.Id,
            content.Latitude,
            content.Longitude,
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            livePeriod: livePeriod
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(this TextContent content,
        TelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo
    )
    {
        string messageText = GetMessageText(content.Content);
        Telegram.Bot.Types.Message ret = await client.SendTextMessageAsync(
            chatId.Id,
            messageText,
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this VideoContent content,
        TelegramBotClient client,
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

        Telegram.Bot.Types.Message ret = await client.SendVideoAsync(
            chatId.Id,
            file,
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            caption: content.Caption,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static async Task<MessageId> Send(
        this VoiceContent content,
        TelegramBotClient client,
        ChatId chatId,
        SendInfo sendInfo,
        IReplyMarkup? markup,
        MessageId? replyTo)
    {
        Telegram.Bot.Types.Message ret = await client.SendVoiceAsync(
            chatId.Id,
            content.Data == null
                ? InputFile.FromFileId(content.FileId)
                : InputFile.FromStream(content.Data, content.FileName),
            replyToMessageId: replyTo?.Id,
            replyMarkup: markup,
            disableNotification: sendInfo.HideNotification,
            protectContent: sendInfo.Protected.IsContentProtected(),
            parseMode: sendInfo.ParseMode.GetParseMode()
        );
        return ret.MessageId;
    }

    public static string GetMessageText(this string text)
    {
        return text.Length > MAX_MESSAGE_TEXT_LENGTH ? text[..(MAX_MESSAGE_TEXT_LENGTH - 1)] : text;
    }

    private const int MAX_MESSAGE_TEXT_LENGTH = 4095;
}