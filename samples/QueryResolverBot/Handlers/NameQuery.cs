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
        if (receivedMessage.Content is not QueryMessageContent content || content.Text.Length < 2)
            return new StateInfo(this);

        await _queryResolver.Response(content.MessageQueryId, new List<QueryMessageResponse>
            {
                new TextQueryMessageResponse($"{content.Text} Ответ1", "Полный ответ 1"),
                new ImageListItemQueryMessageResponse(
                    $"{content.Text} Ответ1",
                    thumbnail:
                    "https://png.pngtree.com/thumb_back/fw800/background/20230610/pngtree-picture-of-a-blue-bird-on-a-black-background-image_2937385.jpg",
                    description: "description")
            },
            Equals(receivedMessage.ChatId, receivedMessage.From.Id)
        );

        return new StateInfo(this);
    }
}