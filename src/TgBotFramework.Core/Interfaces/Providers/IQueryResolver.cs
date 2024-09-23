#region Copyright

/*
 * File: IQueryResolver.cs
 * Author: denisosipenko
 * Created: 2023-12-03
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Отвечает на InlineQuery, который включается в BotFather
/// </summary>
public interface IQueryResolver
{
    Task Response(string queryId, IEnumerable<QueryMessageResponse> results, bool isPersonal = true);
}