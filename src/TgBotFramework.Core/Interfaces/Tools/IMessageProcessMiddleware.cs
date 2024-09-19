#region Copyright

/*
 * File: IMessageProcessMiddleware.cs
 * Author: denisosipenko
 * Created: 2024-04-30
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public interface IMessageProcessMiddleware
{
    Task Process(MessageExecutionContext context, MiddlewareDelegate action);
}