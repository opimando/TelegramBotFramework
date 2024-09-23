#region Copyright

/*
 * File: ITelegramBot.cs
 * Author: denisosipenko
 * Created: 2023-08-09
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Бот
/// </summary>
public interface ITelegramBot
{
    void Start();
    void Stop();
    Task SetCommands(IEnumerable<CommandButton> buttons);
    Task SetDescription(string description);
}