using TgBotFramework.Core;

namespace BasicBot.Handlers;

[TelegramState("Помощь 🥸")]
public class HelpHandler : BaseChatState
{
    public HelpHandler(IEventBus eventsBus) : base(eventsBus)
    {
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        await messenger.Send(
            receivedMessage.ChatId,
            "Сообщение, которое очень помогает пользователю понять что здесь происходит"
        );
        return null;
    }
}