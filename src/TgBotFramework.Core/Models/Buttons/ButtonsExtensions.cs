#region Copyright

/*
 * File: ButtonsExtensions.cs
 * Author: denisosipenko
 * Created: 2023-08-17
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot.Types.ReplyMarkups;
using TgKeyboardButton = Telegram.Bot.Types.ReplyMarkups.KeyboardButton;
using TgBotCommand = Telegram.Bot.Types.BotCommand;

namespace TgBotFramework.Core;

internal static class ButtonsExtensions
{
    public static InlineKeyboardButton? ToInlineButton(this Button btn)
    {
        if (btn is not InlineButton inline) return default;

        return inline.Type switch
        {
            InlineType.Basic => InlineKeyboardButton.WithCallbackData(inline.Title, inline.Data),
            InlineType.Url => InlineKeyboardButton.WithUrl(inline.Title, inline.Data),
            InlineType.Switch => InlineKeyboardButton.WithSwitchInlineQuery(inline.Title, inline.Data),
            InlineType.SwitchCurrentChat => InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(inline.Title,
                inline.Data),
            InlineType.Pay => InlineKeyboardButton.WithPayment(inline.Title),
            InlineType.WebApp =>
                //TODO: addWebApp
                InlineKeyboardButton.WithCallbackData(inline.Title),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static TgKeyboardButton? ToKeyboardButton(this Button btn)
    {
        if (btn is not KeyboardButton keyboard) return default;
        return new TgKeyboardButton(keyboard.Title);
    }

    public static TgBotCommand? ToCommandButton(this Button btn)
    {
        if (btn is not CommandButton command) return default;
        return new TgBotCommand {Command = command.Id, Description = command.Title};
    }
}