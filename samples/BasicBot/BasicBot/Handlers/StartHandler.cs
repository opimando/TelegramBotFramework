using TgBotFramework.Core;

namespace BasicBot.Handlers;

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
            new KeyboardButton("Имя 🧘"),
            new KeyboardButton("Имя2"),
            new KeyboardButton("Помощь 🥸"),
            new KeyboardButton("Пол")
        });

        await messenger.Send(
            receivedMessage.ChatId,
            new SendInfo(new TextContent("Привет, пользователь :)"))
            {
                Buttons = keyboardButtons
            });
        return null;
    }
}