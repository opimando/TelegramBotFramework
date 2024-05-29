#region Copyright

/*
 * File: ExecuteProcessorWrapper.cs
 * Author: denisosipenko
 * Created: 2024-03-20
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ExecuteProcessorWrapper : IMessageProcessMiddleware
{
    private readonly Func<Task<IChatState?>> _processMethod;

    public ExecuteProcessorWrapper(Func<Task<IChatState?>> processMethod)
    {
        _processMethod = processMethod;
    }

    public async Task Process(MessageExecutionContext context, MiddlewareDelegate action)
    {
        context.Result = await _processMethod();
    }
}