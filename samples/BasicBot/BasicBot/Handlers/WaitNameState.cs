using TgBotFramework.Core;

namespace BasicBot.Handlers;

public class WaitNameState : BaseSelfDeleteMessagesState
{
    public WaitNameState(IEventBus eventsBus) : base(eventsBus)
    {
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        Data.MessagesIds.Add(receivedMessage.Id);

        if (receivedMessage.Content is not TextContent text)
        {
            MessageId messageId = await messenger.Send(receivedMessage.ChatId, "Имя должно быть текстом");
            Data.Add(messageId);
            return this;
        }

        if (text.Content.Any(char.IsDigit)) //провалидируем, например, на наличие цифр
        {
            MessageId messageId =
                await messenger.Send(receivedMessage.ChatId, "В имени не может быть цифр, введи заново!");
            Data.Add(messageId);
            return this;
        }

        await messenger.Send(receivedMessage.ChatId, $"Твоё имя сохранено, {text.Content}!");
        return this;
    }
}