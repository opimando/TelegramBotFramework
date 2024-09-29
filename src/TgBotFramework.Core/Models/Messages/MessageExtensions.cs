#region Copyright

/*
 * File: MessageExtensions.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot;
using Telegram.Bot.Types;
using OuterMessage = Telegram.Bot.Types.Message;
using InnerMessage = TgBotFramework.Core.Message;

namespace TgBotFramework.Core;

public static class MessageExtensions
{
    private static ITelegramBotClient? _client;
    public static InnerMessage? GetMessage(this Update telegramMessage, ITelegramBotClient client)
    {
        _client ??= client;
        if (telegramMessage.InlineQuery != null) return telegramMessage.InlineQuery.GetQuery();
        if (telegramMessage.CallbackQuery != null) return telegramMessage.CallbackQuery.GetCallback();
        if (telegramMessage.Message != null) return telegramMessage.Message.GetMessage();
        if (telegramMessage.PollAnswer != null) return telegramMessage.PollAnswer.GetPoll();
        return null;
    }

    public static InnerMessage GetMessage(this OuterMessage message)
    {
        IMessageContent content;

        if (!string.IsNullOrWhiteSpace(message.Text))
        {
            content = new TextContent(message.Text);
        }
        else if (message.Audio != null)
        {
            content = new AudioContent(
                message.Audio.FileId,
                message.Audio.FileName,
                message.Audio.FileUniqueId,
                message.Audio.Duration,
                message.Audio.Title
            );
        }
        else if (message.Contact != null)
        {
            UserId? userId = null;
            if (message.Contact.UserId != null) userId = new UserId(message.Contact.UserId.Value);
            content = new ContactContent(userId, message.Contact.PhoneNumber, message.Contact.LastName,
                message.Contact.FirstName);
        }
        else if (message.Document != null)
        {
            content = new DocumentContent(message.Document.FileId, message.Document.FileUniqueId,
                message.Document.FileName, message.Document.FileSize);
        }
        else if (message.Location != null)
        {
            content = new LocationContent(message.Location.Latitude, message.Location.Longitude,
                message.Location.LivePeriod);
        }
        else if (message.VideoNote != null)
        {
            ImageContent? preview = message.VideoNote.Thumbnail == null
                ? null
                : new ImageContent(message.VideoNote.Thumbnail.FileId, message.VideoNote.Thumbnail.FileUniqueId,
                    message.VideoNote.Thumbnail.FileSize, message.VideoNote.Thumbnail.Width,
                    message.VideoNote.Thumbnail.Height);
            content = new VideoNoteContent(message.VideoNote.FileId, message.VideoNote.FileUniqueId,
                message.VideoNote.FileSize, message.VideoNote.Duration, preview);
        }
        else if (message.Video != null)
        {
            ImageContent? preview = message.Video.Thumbnail == null
                ? null
                : new ImageContent(message.Video.Thumbnail.FileId, message.Video.Thumbnail.FileUniqueId,
                    message.Video.Thumbnail.FileSize, message.Video.Thumbnail.Width, message.Video.Thumbnail.Height);
            content = new VideoContent(message.Video.FileId, message.Video.FileUniqueId, message.Video.FileSize,
                message.Video.Width, message.Video.Height, message.Video.Duration, preview);
        }
        else if (message.Photo != null)
        {
            PhotoSize theBiggestPhoto = message.Photo.MaxBy(s => s.FileSize ?? s.Width)!;

            content = new ImageContent(theBiggestPhoto.FileId, theBiggestPhoto.FileUniqueId,
                theBiggestPhoto.FileSize, theBiggestPhoto.Width, theBiggestPhoto.Height);
        }
        else if (message.Voice != null)
        {
            content = new VoiceContent(message.Voice.FileId,
                message.Voice.FileUniqueId,
                message.Voice.Duration);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(message), "Не удалось получить контет сообщения");
        }

        InnerMessage? reply = null;
        User? forwardedFrom = null;
        if (message.ReplyToMessage != null) reply = message.ReplyToMessage.GetMessage();
        if (message.ForwardFrom != null) forwardedFrom = message.ForwardFrom.GetLocal();

        return new InnerMessage(
            message.MessageId,
            content,
            message.From!.Id,
            message.From.GetLocal(),
            reply,
            forwardedFrom
        );
    }

    public static InnerMessage GetQuery(this InlineQuery query)
    {
        QueryMessageContent content = query.GetContent();
        return new InnerMessage(
            MessageId.NotExistId,
            content,
            query.From.Id,
            query.From.GetLocal()
        );
    }

    public static InnerMessage GetCallback(this CallbackQuery callback)
    {
        CallbackInlineButtonContent content = callback.GetContent();
        return new InnerMessage(
            MessageId.NotExistId,
            content,
            callback.From.Id,
            callback.From.GetLocal()
        );
    }

    public static InnerMessage GetPoll(this PollAnswer pollAnswer)
    {
        VoteChoiceContent content = new(pollAnswer.PollId, pollAnswer.OptionIds.ToList());

        return new InnerMessage(
            MessageId.NotExistId,
            content,
            pollAnswer.User.Id,
            pollAnswer.User.GetLocal()
        );
    }

    public static QueryMessageContent GetContent(this InlineQuery query)
    {
        return new QueryMessageContent(query.Id, query.Query)
        {
            Offset = query.Offset
        };
    }

    public static CallbackInlineButtonContent GetContent(this CallbackQuery query)
    {
        return new CallbackInlineButtonContent(query.Id, query.Data) {Client = _client};
    }
}