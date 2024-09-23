namespace TgBotFramework.Core;

public interface IStateInfo
{
    IChatState? NextState { get; }
    ExecutionType ExecutionType { get; }
}