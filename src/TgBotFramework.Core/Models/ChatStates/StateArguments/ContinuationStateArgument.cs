namespace TgBotFramework.Core;

public class ContinuationStateArgument : StateArgument
{
    public List<Type> NextSteps { get; set; } = new();
    public List<Type> PreviousSteps { get; set; } = new();
}