#region Copyright

/*
 * File: CantReadMessageEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class CantReadMessageEvent : BaseEvent, IStructuredEvent
{
    public CantReadMessageEvent(string messageInfo)
    {
        MessageInfo = messageInfo;
    }

    public string MessageInfo { get; }

    public string Template => "Пришёл запрос, но не смогли достать из него сообщение ({MessageInfo})";
    public object[] Items => new object[] {MessageInfo};
}