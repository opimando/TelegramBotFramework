using TgBotFramework.Core;

namespace BasicBot.Handlers;

[TelegramState("/start", "/test")]
public class StartHandler : BaseChatState
{
    private readonly IChatStateFactory _stateFactory;

    public StartHandler(IChatStateFactory stateFactory)
    {
        _stateFactory = stateFactory;
    }

    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        var data = new ContinuationStateArgument
        {
            NextSteps = new List<Type>
            {
                typeof(WaitAgeState),
                typeof(PrintInfoStateAction)
            }
        };
        var next = await _stateFactory.CreateState<WaitNameState>(data);
        return new StateInfo(next, ExecutionType.RightNow);
    }
}