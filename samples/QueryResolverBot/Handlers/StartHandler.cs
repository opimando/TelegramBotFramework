using TgBotFramework.Core;

namespace QueryResolverBot.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    public StartHandler(IEventBus eventsBus) : base(eventsBus)
    {
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        await messenger.Send(receivedMessage.ChatId, "Начни вводить своё имя");
        return null;
    }
}