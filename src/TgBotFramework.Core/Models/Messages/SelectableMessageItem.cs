#region Copyright

/*
 * File: SelectableMessageItem.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Информация о содержимом InlineButton`а который можно выбрать
/// </summary>
public class SelectableInlineItem
{
    public const string SELECTED_SYMBOL = "✔";

    public SelectableInlineItem(string text, string id)
    {
        Text = text;
        Id = id;
    }

    public string Text { get; }
    public string Id { get; }
    public bool IsSelected { get; set; }

    public override string ToString()
    {
        return IsSelected ? $"{SELECTED_SYMBOL}{Text}" : Text;
    }
}