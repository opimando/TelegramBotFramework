#region Copyright

/*
 * File: BotBuilder.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TgBotFramework.Core;

public class BotBuilder
{
    public List<Action<IServiceCollection, BotBuilder>> RegisterMiddlewares { get; } = new();
    public virtual Action<IServiceCollection, BotBuilder> RegisterAuthProvider { get; set; } = (sc, config) => { };

    protected readonly IStateRegistry StateRegistry = new StateRegistry();

    public virtual Action<IServiceCollection, BotBuilder> RegisterStateStore { get; set; } = (sc, config) =>
        sc.AddSingleton<IChatStateStore>(
            sp =>
            {
                IChatStateStore store = new InMemoryStateStore(sp.GetRequiredService<IChatStateFactory>(),
                    sp.GetRequiredService<IEventBus>(),
                    sp.GetRequiredService<IMessenger>());
                store.SetStates(config.GetRegisteredTypes());
                return store;
            });

    public virtual void Use(Func<IServiceProvider, IMessageProcessMiddleware> middlewareCreationFunc)
    {
        RegisterMiddlewares.Add((sc, _) => sc.AddScoped(middlewareCreationFunc));
    }

    public virtual void UseScoped<T>() where T : IMessageProcessMiddleware
    {
        RegisterMiddlewares.Add((sc, _) => sc.AddScoped(typeof(IMessageProcessMiddleware), typeof(T)));
    }

    public virtual Action<IServiceCollection, BotBuilder> RegisterSpamFilter { get; set; } = (_, _) => { };

    /// <summary>
    /// Зарегистрировать все телеграм-комманды в сборках
    /// </summary>
    /// <param name="assemblies"></param>
    public virtual BotBuilder WithStates(params Assembly[] assemblies)
    {
        StateRegistry.Add(assemblies);
        return this;
    }

    /// <summary>
    /// Зарегистрировать телеграм команды.
    /// У типа должен быть специальный аттрибут <see cref="TelegramStateAttribute"/>>
    /// </summary>
    /// <param name="types"></param>
    public virtual BotBuilder WithStates(params Type[] types)
    {
        StateRegistry.Add(types);
        return this;
    }

    public virtual BotBuilder WithSingleAuthProvider(UserId userId)
    {
        RegisterAuthProvider = (sc, _) => sc.AddSingleton<IAuthProvider>(new SingleUserAuthProvider(userId));
        return this;
    }

    public virtual BotBuilder WithGroupAuthProvider(ChatId groupId)
    {
        RegisterAuthProvider = (sc, _) =>
            sc.AddSingleton<IAuthProvider>(sp =>
                new AuthProviderByContainsInGroup(sp.GetRequiredService<IGroupManager>(), groupId));
        return this;
    }

    public virtual BotBuilder WithAuthProvider(IAuthProvider provider)
    {
        RegisterAuthProvider = (sc, _) => sc.AddSingleton(provider);
        return this;
    }

    public virtual BotBuilder WithStateStore(IChatStateStore stateStore)
    {
        RegisterStateStore = (sc, _) => sc.AddSingleton(stateStore);
        return this;
    }

    public virtual BotBuilder WithSpamFilter(ISpamSenderFilter filter)
    {
        RegisterSpamFilter = (sc, _) => sc.AddSingleton(filter);
        return this;
    }

    public virtual BotBuilder WithSpamFilter(int maxCountPerMinute)
    {
        RegisterSpamFilter = (sc, _) => sc.AddScoped<ISpamSenderFilter>(_ => new SpamSenderFilter(maxCountPerMinute));
        return this;
    }

    public virtual List<TelegramStateInfo> GetRegisteredTypes()
    {
        return StateRegistry.GetRegisteredStates();
    }
}