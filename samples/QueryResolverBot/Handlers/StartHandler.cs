using TgBotFramework.Core;
using ChatAction = TgBotFramework.Core.ChatAction;
using Message = TgBotFramework.Core.Message;

namespace QueryResolverBot.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        await Messenger.Send(receivedMessage.ChatId, ChatAction.Typing);
        await Task.Delay(TimeSpan.FromSeconds(2)); //чтобы увидеть уведомление о том что бот печатает
        await Messenger.Send(receivedMessage.ChatId, new SendInfo(new TextContent("Жми и начинай"))
        {
            Buttons = new InlineButtonGroup(new List<Button>
                {new InlineButton("Начать поиск", "", InlineType.SwitchCurrentChat)})
        });
        return new StateInfo(null);
    }
}