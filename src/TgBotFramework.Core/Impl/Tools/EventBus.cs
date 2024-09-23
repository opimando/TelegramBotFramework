#region Copyright

/*
 * File: EventBus.cs
 * Author: denisosipenko
 * Created: 2023-08-13
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Реализация шины событий.
/// Предусмотрена многопоточная подписка и уведомление
/// </summary>
public class EventBus : IEventBus
{
    private readonly List<EventSubscriptionList> _subscribers = new();
    private readonly ReaderWriterLockSlim _locker = new();

    public void Publish<T>(T stepEvent) where T : BaseEvent
    {
        List<Type> messageTypes = GetBaseTypes(typeof(T));

        _locker.EnterReadLock();
        try
        {
            List<EventSubscriptionList> toNotify = _subscribers.Join(messageTypes, sub => sub.EventTypeForSubscription,
                t => t,
                (sub, t) => sub).ToList();

            foreach (EventSubscriptionList subscription in toNotify) subscription.StartNotify(stepEvent);
        }
        finally
        {
            _locker.ExitReadLock();
        }
    }

    public Guid Subscribe<T>(Action<T> onEvent) where T : BaseEvent
    {
        Type messageType = typeof(T);

        _locker.EnterWriteLock();
        try
        {
            EventSubscriptionList? existSub =
                _subscribers.FirstOrDefault(s => s.EventTypeForSubscription == messageType);
            if (existSub != null) return existSub.Add(onEvent);

            var newSub = new EventSubscriptionList(messageType);
            Guid id = newSub.Add(onEvent);
            _subscribers.Add(newSub);
            return id;
        }
        finally
        {
            _locker.ExitWriteLock();
        }
    }

    public void Unsubscribe(Guid subscriptionId)
    {
        _locker.EnterWriteLock();
        try
        {
            EventSubscriptionList? existSub = _subscribers.FirstOrDefault(s => s.HasSubscription(subscriptionId));
            if (existSub == null) return;

            existSub.RemoveIfExist(subscriptionId);
            if (existSub.GetHandlersCount() == 0)
                _subscribers.Remove(existSub);
        }
        finally
        {
            _locker.ExitWriteLock();
        }
    }

    private List<Type> GetBaseTypes(Type eventItem)
    {
        List<Type> ret = new() {eventItem};
        if (eventItem.BaseType != null) ret.AddRange(GetBaseTypes(eventItem.BaseType));

        return ret;
    }
}