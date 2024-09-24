using TgBotFramework.Core;

namespace BasicBot.Handlers;

public class WaitAgeState : WaitNameState
{
    public WaitAgeState(IChatStateFactory stateFactory) : base(stateFactory)
    {
    }

    protected override async Task OnStateStartInternal(ChatId chatId)
    {
        Data.Add(await Messenger.Send(chatId, "Пришли возвраст"));
    }

    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        if (IsFirstStateInvoke) return new StateInfo(this);

        Data.Add(receivedMessage.Id);
        if (receivedMessage.Content is not TextContent text)
        {
            Data.Add(await Messenger.Send(receivedMessage.ChatId, "Необходимо прислать колчиество лет в виде текста"));
            return new StateInfo(this);
        }

        if (!int.TryParse(text.Content, out int age))
        {
            Data.Add(await Messenger.Send(receivedMessage.ChatId, "Не удалось получить возраст"));
            return new StateInfo(this);
        }
        
        Data.Age = age;
        return await GetNextOrDefault(Data) ?? new StateInfo(this);
    }
}