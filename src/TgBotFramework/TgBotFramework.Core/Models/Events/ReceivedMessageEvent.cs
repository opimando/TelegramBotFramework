#region Copyright

/*
 * File: ReceivedMessageEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ReceivedMessageEvent : BaseEvent, IStructuredEvent
{
    public ReceivedMessageEvent(ChatId chatId, User from, IMessageContent content)
    {
        ChatId = chatId;
        User = from;
        Content = content;
    }

    public User User { get; }
    public ChatId ChatId { get; }
    public IMessageContent Content { get; }

    public string Template => "Получено сообщение '{Content}' от пользователя {User} в чате {ChatId}";
    public object[] Items => new object[] {Content, User, ChatId};
}