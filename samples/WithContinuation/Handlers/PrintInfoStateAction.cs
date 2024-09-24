using TgBotFramework.Core;

namespace BasicBot.Handlers;

public class PrintInfoStateAction : BaseChatState, IChatStateWithData<UserInfoStateArgument>
{
    private UserInfoStateArgument _data = new();

    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        await Messenger.Send(receivedMessage.ChatId, $"Твоё имя: {_data.Name}, возраст: {_data.Age}");
        return new StateInfo(null);
    }

    public UserInfoStateArgument GetData()
    {
        return _data;
    }

    public Task SetData(UserInfoStateArgument data)
    {
        _data = data;
        return Task.CompletedTask;
    }
}