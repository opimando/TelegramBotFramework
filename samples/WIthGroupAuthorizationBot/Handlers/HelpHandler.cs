using TgBotFramework.Core;

namespace WithGroupAuthorizationBot.Handlers;

[TelegramState("Помощь 🥸")]
public class HelpHandler : BaseChatState
{
    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage)
    {
        await Messenger.Send(
            receivedMessage.ChatId,
            "Сообщение, которое очень помогает пользователю понять что здесь происходит"
        );
        return null;
    }
}