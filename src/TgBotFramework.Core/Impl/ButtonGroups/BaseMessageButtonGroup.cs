#region Copyright

/*
 * File: BaseMessageButtonGroup.cs
 * Author: denisosipenko
 * Created: 2023-08-17
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public abstract class BaseMessageButtonGroup : IButtonStructure
{
    public int ButtonsCount => Buttons.Sum(s => s.Count);
    public List<List<Button>> Buttons { get; }

    public BaseMessageButtonGroup(IEnumerable<List<Button>> buttons)
    {
        Buttons = new List<List<Button>>(buttons);
    }

    public BaseMessageButtonGroup(IEnumerable<Button> buttons) : this(new[] {new List<Button>(buttons)})
    {
    }

    public virtual void Add(int levelIndex, Button button)
    {
        if (Buttons.Count == 0) Buttons.Add(new List<Button>());

        if (levelIndex - (Buttons.Count - 1) >= 1)
        {
            Buttons.Add(new List<Button>());
            if (levelIndex - (Buttons.Count - 1) > 1)
                levelIndex = Buttons.Count - 1;
        }

        if (Buttons.Count > levelIndex)
        {
            List<Button> buttonsAtLevel = Buttons.ElementAt(levelIndex);
            buttonsAtLevel.Add(button);
            return;
        }

        Buttons.Add(new List<Button> {button});
    }
}