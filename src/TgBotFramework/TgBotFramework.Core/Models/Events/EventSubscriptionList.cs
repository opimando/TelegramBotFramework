#region Copyright

/*
 * File: EventSubscriptionList.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Список всех подписок на определённый тип событий
/// </summary>
internal class EventSubscriptionList
{
    private readonly List<SubscriptionActionItem> _handlers = new();
    private readonly object _lockObject = new();

    public EventSubscriptionList(Type eventTypeForSubscription)
    {
        EventTypeForSubscription = eventTypeForSubscription;
    }

    public Type EventTypeForSubscription { get; }

    public Guid Add<T>(Action<T> handler) where T : BaseEvent
    {
        lock (_lockObject)
        {
            var newItem = new SubscriptionActionItem
            {
                Action = evt => handler((T) evt)
            };
            _handlers.Add(newItem);
            return newItem.Id;
        }
    }

    public int GetHandlersCount()
    {
        lock (_lockObject)
        {
            return _handlers.Count;
        }
    }

    public void RemoveIfExist(Guid id)
    {
        try
        {
            lock (_lockObject)
            {
                List<SubscriptionActionItem> toRemove = _handlers.Where(s => s.Id == id).ToList();
                foreach (SubscriptionActionItem item in toRemove) _handlers.Remove(item);
            }
        }
        catch (Exception)
        {
            //не нашли и ладно
        }
    }

    public void StartNotify<T>(T eventToNotify) where T : BaseEvent
    {
        List<SubscriptionActionItem> subscribers;

        lock (_lockObject)
        {
            subscribers = _handlers.ToList();
        }

        Task.Run(() =>
        {
            foreach (SubscriptionActionItem subscriber in subscribers)
                try
                {
                    subscriber.Action(eventToNotify);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Ошибка при обработке события {eventToNotify} одним из подписчиков: {ex.Message}");
                }
        });
    }

    public bool HasSubscription(Guid id)
    {
        lock (_lockObject)
        {
            return _handlers.Any(h => h.Id.Equals(id));
        }
    }

    private class SubscriptionActionItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Action<BaseEvent> Action { get; init; }
    }
}