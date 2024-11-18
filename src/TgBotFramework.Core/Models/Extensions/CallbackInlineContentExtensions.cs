using Telegram.Bot;

namespace TgBotFramework.Core;

public static class CallbackInlineContentExtensions
{
    public static Task Answer(this CallbackInlineButtonContent callback, string? text = null, bool? showAlert = null)
    {
        return callback?.Client?.AnswerCallbackQuery(callback.QueryId, text, showAlert ?? false) ?? Task.CompletedTask;
    }
}