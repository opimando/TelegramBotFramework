namespace TgBotFramework.Core;

public abstract class BaseContinuationState : BaseChatState, IChatStateWithData<ContinuationStateArgument>
{
    protected readonly IChatStateFactory StateFactory;
    protected ContinuationStateArgument ContinuationData = new();

    public BaseContinuationState(IChatStateFactory stateFactory)
    {
        StateFactory = stateFactory;
    }

    protected virtual async Task<IStateInfo?> GetNextOrDefault(params StateArgument[] nextContexts)
    {
        if (ContinuationData.NextSteps.Count == 0) return null;
        Type nextType = ContinuationData.NextSteps.First();
        
        ContinuationData.NextSteps.RemoveAt(0);
        ContinuationData.PreviousSteps.Add(GetType());

        var nexts = nextContexts.ToList();

        if (!nexts.Contains(ContinuationData)) nexts.Add(ContinuationData);

        IChatState next = await StateFactory.CreateState(nextType, nexts.ToArray());
        return new StateInfo(next, ExecutionType.RightNow);
    }

    protected virtual async Task<IStateInfo?> GetPreviousOrDefault(params StateArgument[] nextContexts)
    {
        Type? nextType = ContinuationData.PreviousSteps.LastOrDefault();
        if (nextType == null) return null;
        ContinuationData.PreviousSteps.RemoveAt(ContinuationData.PreviousSteps.Count - 1);
        ContinuationData.NextSteps.Insert(0, GetType());

        var nexts = nextContexts.ToList();

        if (!nexts.Contains(ContinuationData)) nexts.Add(ContinuationData);

        IChatState next = await StateFactory.CreateState(nextType, nexts.ToArray());
        return new StateInfo(next, ExecutionType.RightNow);
    }

    public virtual ContinuationStateArgument GetData()
    {
        return ContinuationData;
    }

    public virtual Task SetData(ContinuationStateArgument data)
    {
        ContinuationData = data;
        return Task.CompletedTask;
    }
}