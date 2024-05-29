#region Copyright

/*
 * File: ServiceCollectionExtenions.cs
 * Author: denisosipenko
 * Created: 2023-11-15
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TgBotFramework.Persistent;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection WithPersistent(
        this IServiceCollection serviceCollection,
        string connectionString
    )
    {
        serviceCollection.AddScoped(typeof(IStateRepository), typeof(StateRepository));
        serviceCollection.AddDbContextFactory<TelegramContext>((_, builder) =>
        {
            builder.UseNpgsql(connectionString);
        });
        return serviceCollection;
    }

    public static async Task MigrateStateStore(this IServiceProvider serviceProvider)
    {
        await using TelegramContext context = await serviceProvider
            .GetRequiredService<IDbContextFactory<TelegramContext>>()
            .CreateDbContextAsync();
        await context.Database.MigrateAsync();
    }
}