#region Copyright

/*
 * File: InlineButtonGroup.cs
 * Author: denisosipenko
 * Created: 2023-08-17
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class InlineButtonGroup : BaseMessageButtonGroup
{
    public InlineButtonGroup(IEnumerable<List<InlineButton>> buttons) : base(buttons.Select(s =>
        s.OfType<Button>().ToList()))
    {
    }

    public InlineButtonGroup(IEnumerable<Button> buttons) : base(buttons)
    {
    }
}