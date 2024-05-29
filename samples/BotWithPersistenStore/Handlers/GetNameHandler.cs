#region Copyright

/*
 * File: GetNameHandler.cs
 * Author: denisosipenko
 * Created: 2024-04-29
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

using TgBotFramework.Core;

namespace BotWithPersistenStore.Handlers;

public class GetNameHandler : BaseSelfDeleteMessagesState
{
    private readonly IChatStateFactory _stateFactory;

    public GetNameHandler(IEventBus eventsBus, IChatStateFactory stateFactory) : base(eventsBus)
    {
        _stateFactory = stateFactory;
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        Data.Add(receivedMessage.Id);

        if ((receivedMessage.Content as TextContent)!.Content
            .Any(char.IsDigit)) //провалидируем, например, на наличие цифр
        {
            MessageId messageId =
                await messenger.Send(receivedMessage.ChatId, "В имени не может быть цифр, введи заново!");
            Data.Add(messageId);
            return this;
        }

        string data = (receivedMessage.Content as TextContent)!.Content;

        var next = await _stateFactory.CreateState<GetLastNameHandler>(
            new LastnameArgument {FirstName = data}
        );

        return next;
    }
}