using TgBotFramework.Core;

namespace BasicBot.Handlers;

[TelegramState("Помощь 🥸")]
public class HelpHandler : BaseChatState
{
    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        await Messenger.Send(
            receivedMessage.ChatId,
            "Сообщение, которое очень помогает пользователю понять что здесь происходит"
        );
        return new StateInfo(null);
    }
}