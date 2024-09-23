#region Copyright

/*
 * File: SetNameHandler.cs
 * Author: denisosipenko
 * Created: 2024-04-29
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

using TgBotFramework.Core;

namespace BotWithPersistenStore.Handlers;

[TelegramState("Имя 🧘")]
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
        var nextState = await _stateFactory.CreateState<GetNameHandler>(
            new MessageToDeleteArgument
            {
                MessagesIds = new List<MessageId>
                {
                    messageId,
                    receivedMessage.Id
                }
            }
        );
        return new StateInfo(nextState);
    }
}