#region Copyright

/*
 * File: VoteChoiseContent.cs
 * Author: denisosipenko
 * Created: 2024-01-16
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class VoteChoiceContent : IMessageContent
{
    public string PollId { get; }
    public List<int> OptionsIds { get; }

    public VoteChoiceContent(string pollId, List<int> optionsIds)
    {
        PollId = pollId;
        OptionsIds = optionsIds;
    }

    public bool IsEmpty()
    {
        return !OptionsIds.Any();
    }

    public override string ToString()
    {
        return $"*Выбор в голосовании* voteId: {PollId}; options: {string.Join(",", OptionsIds)}";
    }
}