#region Copyright

/*
 * File: CommandButton.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public record CommandButton(string Id, string Title) : KeyboardButton(Title);