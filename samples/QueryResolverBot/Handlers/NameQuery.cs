using TgBotFramework.Core;

namespace QueryResolverBot.Handlers;

[TelegramQueryState]
public class NameQuery : BaseChatState
{
    private readonly IQueryResolver _queryResolver;

    public NameQuery(IQueryResolver queryResolver)
    {
        _queryResolver = queryResolver;
    }

    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        if (receivedMessage.Content is not QueryMessageContent content) return new StateInfo(this);

        await _queryResolver.Response(content.MessageQueryId, new List<QueryMessageResponse>
        {
            new TextQueryMessageResponse("Ответ1", "Полный ответ 1"),
            new TextQueryMessageResponse("Ответ2", "Полный ответ 2")
        }, Equals(receivedMessage.ChatId, receivedMessage.From.Id));

        return new StateInfo(this);
    }
}