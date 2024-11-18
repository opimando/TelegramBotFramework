#region Copyright

/*
 * File: SendInfo.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class SendInfo
{
    public SendInfo(IMessageContent content)
    {
        Content = content;
    }

    public IMessageContent Content { get; }
    public bool HideNotification { get; set; }
    public ContentProtectState Protected { get; set; }
    public ParseMode ParseMode { get; set; } = ParseMode.Markdown;
    public IButtonStructure? Buttons { get; set; }

    public static implicit operator SendInfo(string text)
    {
        return new SendInfo(new TextContent(text));
    }

    public static implicit operator SendInfo(ChatAction action)
    {
        return new SendInfo(new ChatActionContent(action));
    }

    public override string ToString()
    {
        return Content.ToString()!;
    }
}