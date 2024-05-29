#region Copyright

/*
 * File: IEventBus.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Шина событий
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Подписаться на тип сообщения.
    /// Также подписка оформиться и на все отнаследованные от указанного типа события.
    /// BaseEvent -> StartEvent: подписавшись на BaseEvent уведомление получишь при публикации StartEvent
    /// </summary>
    /// <param name="onEventReceived"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Guid Subscribe<T>(Action<T> onEventReceived) where T : BaseEvent;

    void Unsubscribe(Guid subscriptionId);

    void Publish<T>(T newEvent) where T : BaseEvent;
}