#region Copyright

/*
 * File: InStateHandlerError.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class InStateHandlerError : ErrorEvent
{
    public IChatState State { get; }
    public ChatId ChatId { get; }

    public InStateHandlerError(Exception exception, IChatState state, ChatId chatId) : base(exception)
    {
        State = state;
        ChatId = chatId;
    }

    public InStateHandlerError(string description, IChatState state, ChatId chatId) : base(description)
    {
        State = state;
        ChatId = chatId;
    }

    public InStateHandlerError(Exception exception, string description, IChatState state, ChatId chatId) : base(
        exception, description)
    {
        State = state;
        ChatId = chatId;
    }

    public override string ToString()
    {
        return $"Ошибка в ходе обработки сообщения в чате {ChatId}: {Description ?? Exception?.Message}";
    }

    public override string Template =>
        "Ошибка в ходе обработки сообщения в чате {ChatId} состоянии {@State}: {@Exception} {Description}";

    public override object?[] Items => new object?[] {ChatId, State, Exception, Description};
}