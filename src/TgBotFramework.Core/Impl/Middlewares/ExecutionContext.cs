#region Copyright

/*
 * File: ExecutionContext.cs
 * Author: denisosipenko
 * Created: 2024-03-20
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class MessageExecutionContext
{
    public Message Message { get; set; }
    public IChatState? ExecutingState { get; set; }
    public IStateInfo Result { get; set; }
}