#region Copyright

/*
 * File: PollStarted.cs
 * Author: denisosipenko
 * Created: 2024-01-16
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class PollStarted : BaseEvent, IStructuredEvent
{
    public string PollId { get; }
    public List<string> Options { get; }
    public string Question { get; }

    public PollStarted(string pollId, List<string> options, string question)
    {
        PollId = pollId;
        Options = options;
        Question = question;
    }

    public string Template => "Стартовало голосование {PollId} с вопросом '{Question}' и вариантами: {Options}";
    public object[] Items => new object[] {PollId, Question, string.Join(",", Options)};
}