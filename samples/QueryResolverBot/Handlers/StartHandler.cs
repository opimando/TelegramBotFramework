using TgBotFramework.Core;

namespace QueryResolverBot.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage)
    {
        await Messenger.Send(receivedMessage.ChatId, "Начни вводить своё имя");
        return null;
    }
}