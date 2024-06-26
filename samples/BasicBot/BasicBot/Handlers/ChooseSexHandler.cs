﻿#region Copyright

/*
 * File: ChooseSexHandler.cs
 * Author: denisosipenko
 * Created: 2024-04-29
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

using TgBotFramework.Core;

namespace BasicBot.Handlers;

[TelegramState("/sex", "пол")]
public class ChooseSexHandler : BaseChatState
{
    public ChooseSexHandler(IEventBus eventsBus) : base(eventsBus)
    {
    }

    protected override async Task OnStateStartInternal(IMessenger messenger, ChatId chatId)
    {
        await messenger.Send(chatId, new SendInfo(new TextContent("Выбери пол"))
        {
            Buttons = new InlineButtonGroup(new[]
                {new InlineButton("Муж", "male"), new InlineButton("Жен", "female")})
        });
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        if (IsFirstStateInvoke) return this;

        if (receivedMessage.Content is not CallbackInlineButtonContent content)
        {
            await messenger.Send(receivedMessage.ChatId, "Выбери пол :(");
            return this;
        }

        string choosed = content.Data == "male" ? "мужской" :
            content.Data == "female" ? "женский" : "это что за пол такой?";
        await messenger.Send(receivedMessage.ChatId, $"Ты выбрал {choosed}");
        return null;
    }
}