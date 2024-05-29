using TgBotFramework.Core;

namespace WithGroupAuthorizationBot.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    public StartHandler(IEventBus eventsBus) : base(eventsBus)
    {
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        var keyboardButtons = new KeyboardButtonGroup(new[]
        {
            new KeyboardButton("Имя"),
            new KeyboardButton("Помощь 🥸")
        });

        await messenger.Send(
            receivedMessage.ChatId,
            new SendInfo(new TextContent("Привет, авторизованный пользователь :)"))
            {
                Buttons = keyboardButtons
            });
        return null;
    }
}