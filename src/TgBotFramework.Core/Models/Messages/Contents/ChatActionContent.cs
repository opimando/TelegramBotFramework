namespace TgBotFramework.Core;

public class ChatActionContent : IMessageContent
{
    public ChatAction Action { get; }

    public ChatActionContent(ChatAction action)
    {
        Action = action;
    }

    public bool IsEmpty()
    {
        return false;
    }

    public static implicit operator ChatActionContent(ChatAction action)
    {
        return new ChatActionContent(action);
    }

    public override string ToString()
    {
        return $"*Действие*: '{Action.ToString()}'";
    }
}