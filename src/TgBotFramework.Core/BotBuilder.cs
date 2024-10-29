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
    public virtual Action<IServiceCollection, BotBuilder> AuthProviderRegistrationFunction { get; set; } =
        (sc, config) => { };

    public List<Action<IServiceCollection, BotBuilder>> MiddlewaresRegistrationFunctions { get; } = new();

    public ExceptionPolicy Exceptions { get; set; } = new(); 

    public virtual Action<IServiceCollection, BotBuilder> SpamFilterRegistrationFunction { get; set; } =
        (serviceCollection, builder) => { };

    protected readonly IStateRegistry StateRegistry = new StateRegistry();

    public virtual Action<IServiceCollection, BotBuilder> StateStoreRegistrationFunction { get; set; } = (sc, config) =>
        sc.AddSingleton<IChatStateStore>(
            sp =>
            {
                IChatStateStore store = new InMemoryStateStore(sp.GetRequiredService<IChatStateFactory>(),
                    sp.GetRequiredService<IEventBus>());
                store.SetStates(config.GetRegisteredTypes());
                return store;
            });

    public virtual void Use(Func<IServiceProvider, IMessageProcessMiddleware> middlewareCreationFunc)
    {
        MiddlewaresRegistrationFunctions.Add((sc, _) => sc.AddSingleton(middlewareCreationFunc));
    }

    public virtual void UseTransient<T>() where T : IMessageProcessMiddleware
    {
        MiddlewaresRegistrationFunctions.Add((sc, _) => sc.AddTransient(typeof(IMessageProcessMiddleware), typeof(T)));
    }

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
        AuthProviderRegistrationFunction =
            (sc, _) => sc.AddSingleton<IAuthProvider>(new SingleUserAuthProvider(userId));
        return this;
    }

    public virtual BotBuilder WithGroupAuthProvider(ChatId groupId)
    {
        AuthProviderRegistrationFunction = (sc, _) =>
            sc.AddSingleton<IAuthProvider>(sp =>
                new AuthProviderByContainsInGroup(sp.GetRequiredService<IGroupManager>(), groupId));
        return this;
    }

    public virtual BotBuilder WithAuthProvider(IAuthProvider provider)
    {
        AuthProviderRegistrationFunction = (sc, _) => sc.AddSingleton(provider);
        return this;
    }

    public virtual BotBuilder WithStateStore(IChatStateStore stateStore)
    {
        StateStoreRegistrationFunction = (sc, _) => sc.AddSingleton(stateStore);
        return this;
    }

    public virtual BotBuilder WithSpamFilter(ISpamSenderFilter filter)
    {
        SpamFilterRegistrationFunction = (sc, _) => sc.AddSingleton(filter);
        return this;
    }

    public virtual BotBuilder WithSpamFilter(int maxCountPerMinute)
    {
        SpamFilterRegistrationFunction = (sc, _) =>
            sc.AddTransient<ISpamSenderFilter>(_ => new SpamSenderFilter(maxCountPerMinute));
        return this;
    }

    public virtual List<TelegramStateInfo> GetRegisteredTypes()
    {
        return StateRegistry.GetRegisteredStates();
    }
}