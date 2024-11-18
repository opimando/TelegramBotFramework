using TgBotFramework.Core;

namespace BasicBot.Handlers;

public class WaitNameState : BaseSelfDeleteMessagesState
{
    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        if (IsFirstStateInvoke) return new StateInfo(this);
        
        Data.MessagesIds.Add(receivedMessage.Id);

        if (receivedMessage.Content is not TextContent text)
        {
            MessageId messageId = await Messenger.Send(receivedMessage.ChatId, "Имя должно быть текстом");
            Data.Add(messageId);
            return new StateInfo(this);
        }

        if (text.Content.Any(char.IsDigit)) //провалидируем, например, на наличие цифр
        {
            MessageId messageId =
                await Messenger.Send(receivedMessage.ChatId, "В имени не может быть цифр, введи заново!");
            Data.Add(messageId);
            return new StateInfo(this);
        }

        await Messenger.Send(receivedMessage.ChatId, $"Твоё имя сохранено, {text.Content}!");
        return new StateInfo(null);
    }
}