#region Copyright

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
    protected override async Task OnStateStartInternal(ChatId chatId)
    {
        await Messenger.Send(chatId, new SendInfo(new TextContent("Выбери пол"))
        {
            Buttons = new InlineButtonGroup(new[]
                {new InlineButton("Муж", "male"), new InlineButton("Жен", "female")})
        });
    }

    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        if (IsFirstStateInvoke) return new StateInfo(this);

        if (receivedMessage.Content is not CallbackInlineButtonContent content)
        {
            await Messenger.Send(receivedMessage.ChatId, "Выбери пол :(");
            return new StateInfo(this);
        }

        string choosed = content.Data == "male" ? "мужской" :
            content.Data == "female" ? "женский" : "это что за пол такой?";
        await Messenger.Send(receivedMessage.ChatId, $"Ты выбрал {choosed}");
        return new StateInfo(null);
    }
}