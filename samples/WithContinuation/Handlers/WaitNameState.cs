using TgBotFramework.Core;

namespace BasicBot.Handlers;

public class WaitNameState : BaseContinuationState, IChatStateWithData<UserInfoStateArgument>
{
    protected UserInfoStateArgument Data = new();

    public WaitNameState(IChatStateFactory stateFactory) : base(stateFactory)
    {
    }

    protected override async Task OnStateStartInternal(ChatId chatId)
    {
        Data.Add(await Messenger.Send(chatId, "Пришли имя"));
    }

    protected override async Task OnStateExitInternal(ChatId chatId)
    {
        var tasks = Data.MessagesIds.Select(m => Messenger.Delete(chatId, m));
        await Task.WhenAll(tasks);
    }

    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        if (IsFirstStateInvoke) return new StateInfo(this);

        Data.Add(receivedMessage.Id);
        if (receivedMessage.Content is not TextContent text)
        {
            Data.Add(await Messenger.Send(receivedMessage.ChatId, "Необходимо прислать имя в виде текста"));
            return new StateInfo(this);
        }

        Data.Name = text.Content;
        return await GetNextOrDefault(Data) ?? new StateInfo(this);
    }

    UserInfoStateArgument IChatStateWithData<UserInfoStateArgument>.GetData()
    {
        return Data;
    }

    Task IChatStateWithData<UserInfoStateArgument>.SetData(UserInfoStateArgument data)
    {
        Data = data;
        return Task.CompletedTask;
    }
}