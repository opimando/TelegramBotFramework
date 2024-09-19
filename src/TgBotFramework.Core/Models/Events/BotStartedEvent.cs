#region Copyright

/*
 * File: BotStartedEvent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class BotStartedEvent : BaseEvent
{
    public override string ToString()
    {
        return "Телеграм бот стартовал";
    }
}