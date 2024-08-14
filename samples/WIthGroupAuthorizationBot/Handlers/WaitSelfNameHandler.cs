﻿using TgBotFramework.Core;

namespace WithGroupAuthorizationBot.Handlers;

[TelegramState("имя")]
public class WaitSelfNameHandler : BaseSelfDeleteMessagesState
{
    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage)
    {
        if (IsFirstStateInvoke) return this;

        if (receivedMessage.Content is not TextContent text)
        {
            MessageId messageId = await Messenger.Send(receivedMessage.ChatId, "Имя должно быть текстом");
            Data.Add(messageId);
            return this;
        }

        if (text.Content.Any(char.IsDigit)) //провалидируем, например, на наличие цифр
        {
            MessageId messageId =
                await Messenger.Send(receivedMessage.ChatId, "В имени не может быть цифр, введи заново!");
            Data.Add(messageId);
            return this;
        }

        MessageId successMessage =
            await Messenger.Send(receivedMessage.ChatId, $"Твоё имя сохранено, {text.Content}!");
        //можно сохранить имя или передать его в следующий шаг

        return null;
    }

    protected override async Task OnStateStartInternal(ChatId chatId)
    {
        Data.Add(await Messenger.Send(chatId, "Введи своё имя"));
    }
}