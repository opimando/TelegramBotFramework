#region Copyright

/*
 * File: QueryResolver.cs
 * Author: denisosipenko
 * Created: 2023-12-03
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;

namespace TgBotFramework.Core;

public class QueryResolver : IQueryResolver
{
    private readonly ITelegramBotClient _client;
    private readonly IEventBus _eventsBus;

    public QueryResolver(ITelegramBotClient client, IEventBus eventsBus)
    {
        _client = client;
        _eventsBus = eventsBus;
    }

    public async Task Response(string queryId, IEnumerable<QueryMessageResponse> results, bool isPersonal = true)
    {
        try
        {
            List<InlineQueryResult> queryResults = GetQueryResults(results).ToList();
            await _client.AnswerInlineQueryAsync(queryId, queryResults, isPersonal: isPersonal);
        }
        catch (Exception ex)
        {
            _eventsBus.Publish(new ErrorEvent(ex, $"Ошибка при отправке ответа на запрос {queryId}"));
        }
    }

    private IEnumerable<InlineQueryResult> GetQueryResults(IEnumerable<QueryMessageResponse> results)
    {
        return results.Select(result => result switch
        {
            TextQueryMessageResponse text => new InlineQueryResultArticle(text.Id, text.Title,
                new InputTextMessageContent(text.Text) {ParseMode = text.ParseMode.GetParseMode()}),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result,
                "Не удалось распознать ответ на Inline Query")
        });
    }
}