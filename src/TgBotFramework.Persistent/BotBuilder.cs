#region Copyright

/*
 * File: BotBuilder.cs
 * Author: denisosipenko
 * Created: 2023-11-15
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.Core;

namespace TgBotFramework.Persistent;

public static class BotBuilderExtensions
{
    public static BotBuilder WithPersistentStore(this BotBuilder builder)
    {
        builder.StateStoreRegistrationFunction = (sc, config) =>
            sc.AddTransient<IChatStateStore>(sp =>
            {
                var store = new PersistentChatStateStore(
                    sp.GetRequiredService<IStateRepository>(),
                    sp.GetRequiredService<IChatStateFactory>(),
                    sp.GetRequiredService<IEventBus>()
                );
                store.SetStates(config.GetRegisteredTypes());
                return store;
            });
        return builder;
    }
}