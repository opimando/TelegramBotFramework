using TgBotFramework.Core;

namespace BasicBot.Handlers;

public class UserInfoStateArgument : MessageToDeleteArgument
{
    public string? Name { get; set; }
    public int? Age { get; set; }
}