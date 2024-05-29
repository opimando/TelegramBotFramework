#region Copyright

/*
 * File: KeyboardButtonGroup.cs
 * Author: denisosipenko
 * Created: 2023-08-17
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class KeyboardButtonGroup : BaseMessageButtonGroup
{
    public KeyboardButtonGroup(IEnumerable<List<KeyboardButton>> buttons) : base(
        buttons.Select(b => b.OfType<Button>().ToList()))
    {
    }

    public KeyboardButtonGroup(IEnumerable<KeyboardButton> buttons) : base(buttons)
    {
    }
}