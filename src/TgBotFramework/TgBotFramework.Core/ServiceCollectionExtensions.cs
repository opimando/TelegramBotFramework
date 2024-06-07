#region Copyright

/*
 * File: ServiceCollectionExtensions.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace TgBotFramework.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InitializeBot(this IServiceCollection serviceCollection,
        string apiKey,
        Action<BotBuilder>? onConfiguration = null
    )
    {
        var config = new BotBuilder();
        onConfiguration?.Invoke(config);

        config.StateStoreRegistrationFunction(serviceCollection, config);
        config.SpamFilterRegistrationFunction(serviceCollection, config);
        config.AuthProviderRegistrationFunction(serviceCollection, config);

        serviceCollection.AddSingleton<IMessenger>(sp => new Messenger(
            apiKey,
            sp.GetRequiredService<IEventBus>(),
            spamFilter: sp.GetService<ISpamSenderFilter>()
        ));
        serviceCollection.AddSingleton(typeof(ITelegramBotClient),
            sp => (sp.GetRequiredService<IMessenger>() as Messenger)!.Client);

        serviceCollection.AddSingleton<TelegramBot>(sp => new TelegramBot(
            (sp.GetRequiredService<IMessenger>() as Messenger)!.Client,
            sp.GetRequiredService<IEventBus>(),
            sp.GetRequiredService<IMessageProcessQueue>(),
            sp.GetService<IAuthProvider>()
        ));

        serviceCollection.AddSingleton(typeof(IEventBus), typeof(EventBus));
        serviceCollection.AddSingleton(typeof(IMessageProcessor), typeof(MessageProcessor));
        serviceCollection.AddSingleton(typeof(IMessageProcessQueue), typeof(MessageProcessQueue));
        serviceCollection.AddSingleton(typeof(IChatStateFactory), typeof(ChatStateFactory));
        serviceCollection.AddSingleton(typeof(IGroupManager), typeof(GroupManager));
        serviceCollection.AddSingleton(typeof(IQueryResolver), typeof(QueryResolver));

        foreach (Action<IServiceCollection, BotBuilder> middleware in config.MiddlewaresRegistrationFunctions)
            middleware.Invoke(serviceCollection, config);

        return serviceCollection;
    }
}