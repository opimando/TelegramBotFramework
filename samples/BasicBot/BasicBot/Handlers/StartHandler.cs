using TgBotFramework.Core;

namespace BasicBot.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        var keyboardButtons = new KeyboardButtonGroup(new[]
        {
            new KeyboardButton("Имя 🧘"),
            new KeyboardButton("Имя2"),
            new KeyboardButton("Помощь 🥸"),
            new KeyboardButton("Пол")
        });

        await Messenger.Send(
            receivedMessage.ChatId,
            new SendInfo(new TextContent("Привет, пользователь :)"))
            {
                Buttons = keyboardButtons
            });
        return new StateInfo(null);
    }
}