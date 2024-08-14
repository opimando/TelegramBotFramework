using TgBotFramework.Core;

namespace WithGroupAuthorizationBot.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage)
    {
        var keyboardButtons = new KeyboardButtonGroup(new[]
        {
            new KeyboardButton("Имя"),
            new KeyboardButton("Помощь 🥸")
        });

        await Messenger.Send(
            receivedMessage.ChatId,
            new SendInfo(new TextContent("Привет, авторизованный пользователь :)"))
            {
                Buttons = keyboardButtons
            });
        return null;
    }
}