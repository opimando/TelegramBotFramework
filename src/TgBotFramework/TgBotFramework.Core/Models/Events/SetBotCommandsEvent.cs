#region Copyright

/*
 * File: SetBotCommandsEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class SetBotCommandsEvent : BaseEvent
{
    public SetBotCommandsEvent(List<CommandButton> commands)
    {
        Commands = commands;
    }

    public List<CommandButton> Commands { get; }

    public override string ToString()
    {
        return $"Установлены команды бота: {string.Join("; ", Commands)}";
    }
}