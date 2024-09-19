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

        serviceCollection.AddSingleton<ITelegramBotClient>(
            _ => new TelegramBotClient(apiKey) {Timeout = TimeSpan.FromMinutes(1)}
        );

        serviceCollection.AddTransient<IMessenger, Messenger>();
        serviceCollection.AddSingleton<TelegramBot>();

        serviceCollection.AddSingleton<IEventBus, EventBus>();
        serviceCollection.AddSingleton<IMessageProcessQueue, MessageProcessQueue>();

        serviceCollection.AddTransient<IMessageProcessor, MessageProcessor>();
        serviceCollection.AddTransient<IChatStateFactory, ChatStateFactory>();
        serviceCollection.AddTransient<IGroupManager, GroupManager>();
        serviceCollection.AddTransient<IQueryResolver, QueryResolver>();
        serviceCollection.AddTransient<IFileProvider, FileProvider>();

        foreach (Action<IServiceCollection, BotBuilder> middleware in config.MiddlewaresRegistrationFunctions)
            middleware.Invoke(serviceCollection, config);

        return serviceCollection;
    }
}