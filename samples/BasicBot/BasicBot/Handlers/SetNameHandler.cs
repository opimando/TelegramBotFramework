using TgBotFramework.Core;

namespace BasicBot.Handlers;

/// <summary>
/// Состояние которое отправит сообщение о том что нужно ввести имя и уйдёт в шаг ожидания (WaitNameState).
/// Вместо такого двойного состояния можно пользоваться другим способом показанным в WaitSelfNameHandler.
/// </summary>
[TelegramState(acceptedMessages: "Имя 🧘")]
public class SetNameHandler : BaseChatState
{
    private readonly IChatStateFactory _stateFactory;

    public SetNameHandler(IChatStateFactory stateFactory)
    {
        _stateFactory = stateFactory;
    }

    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        MessageId messageId = await Messenger.Send(receivedMessage.ChatId, "Введи своё имя");
        var nextState = await _stateFactory.CreateState<WaitNameState>(
            new MessageToDeleteArgument
            {
                MessagesIds = new List<MessageId> {messageId, receivedMessage.Id}
            }
        );
        return new StateInfo(nextState, ExecutionType.RightNow);
    }
}