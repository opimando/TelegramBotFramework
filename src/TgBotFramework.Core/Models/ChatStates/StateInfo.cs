namespace TgBotFramework.Core;

public class StateInfo : IStateInfo
{
    public IChatState? NextState { get; set; }
    public ExecutionType ExecutionType { get; set; } = ExecutionType.NextInvoke;

    public StateInfo(IChatState? nextState, ExecutionType executionType = ExecutionType.NextInvoke)
    {
        NextState = nextState;
        ExecutionType = executionType;
    }
}