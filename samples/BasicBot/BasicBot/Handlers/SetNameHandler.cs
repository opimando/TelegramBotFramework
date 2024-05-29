using TgBotFramework.Core;

namespace BasicBot.Handlers;

/// <summary>
/// Состояние которое отправит сообщение о том что нужно ввести имя и уйдёт в шаг ожидания (WaitNameState).
/// Вместо такого двойного состояния можно пользоваться другим способом показаным в WaitSelfNameHandler.
/// </summary>
[TelegramState(acceptedMessages: "Имя 🧘")]
public class SetNameHandler : BaseChatState
{
    private readonly IChatStateFactory _stateFactory;

    public SetNameHandler(IEventBus eventsBus, IChatStateFactory stateFactory) : base(eventsBus)
    {
        _stateFactory = stateFactory;
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        MessageId messageId = await messenger.Send(receivedMessage.ChatId, "Введи своё имя");
        var nextState = await _stateFactory.CreateState<WaitNameState>(
            new MessageToDeleteArgument
            {
                MessagesIds = new List<MessageId> {messageId, receivedMessage.Id}
            }
        );
        return nextState;
    }
}