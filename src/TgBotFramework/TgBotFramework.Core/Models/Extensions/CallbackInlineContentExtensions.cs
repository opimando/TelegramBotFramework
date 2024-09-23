﻿using Telegram.Bot;

namespace TgBotFramework.Core.Extensions;

public static class CallbackInlineContentExtensions
{
    public static Task Answer(this CallbackInlineButtonContent callback, string? text = null, bool? showAlert = null)
    {
        return callback.Client.AnswerCallbackQueryAsync(callback.QueryId, text, showAlert);
    }
}