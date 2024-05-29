#region Copyright

/*
 * File: MessageButtonGroupExtensions.cs
 * Author: denisosipenko
 * Created: 2023-08-17
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot.Types.ReplyMarkups;
using TgKeyboardButton = Telegram.Bot.Types.ReplyMarkups.KeyboardButton;

namespace TgBotFramework.Core;

internal static class MessageButtonGroupExtensions
{
    public static IReplyMarkup? ToMarkup(this IButtonStructure buttons)
    {
        if (buttons is not BaseMessageButtonGroup buttonStructure) throw new NotImplementedException();

        return buttons switch
        {
            InlineButtonGroup inline => GetInlineMarkup(inline),
            KeyboardButtonGroup keyboard => GetReplyMarkup(keyboard),
            _ => null
        };
    }

    private static IReplyMarkup GetInlineMarkup(InlineButtonGroup buttons)
    {
        List<List<Button>> buttonsByLevels = buttons.Buttons;

        List<InlineKeyboardButton[]> retByLvls =
            buttonsByLevels.Select(lvl =>
                lvl.Select(s => s.ToInlineButton()).Where(s => s != null).Select(s => s!).ToArray()).ToList();

        return new InlineKeyboardMarkup(retByLvls);
    }

    private static IReplyMarkup GetReplyMarkup(KeyboardButtonGroup buttons)
    {
        List<List<Button>> buttonsByLevels = buttons.Buttons;

        List<TgKeyboardButton[]> retByLvls =
            buttonsByLevels.Select(lvl =>
                lvl.Select(s => s.ToKeyboardButton()).Where(s => s != null).Select(s => s!).ToArray()).ToList();

        return new ReplyKeyboardMarkup(retByLvls)
        {
            ResizeKeyboard = true,
            Selective = true
        };
    }
}