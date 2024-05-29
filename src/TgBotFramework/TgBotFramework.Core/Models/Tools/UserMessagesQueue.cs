#region Copyright

/*
 * File: UserMessagesQueue.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class UserMessagesQueue : Queue<Message>
{
    public SemaphoreSlim ExecutorLock { get; } = new(1, 1);
}