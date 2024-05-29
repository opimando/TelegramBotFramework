using System.Reflection;
using BotWithPersistenStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgBotFramework.Core;
using TgBotFramework.Core;
using TgBotFramework.Persistent;

IHost? builded = new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("settings.json", true, true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var settings = context.Configuration.Get<Settings>();
        if (settings == null)
            throw new ArgumentNullException(nameof(settings), "Не удалось получить настройки приложения");

        services
            .InitializeBot(settings.ApiKey, builder =>
            {
                builder.WithPersistentStore();
                builder.WithSingleAuthProvider(new UserId(settings.UserId));
                builder.WithStates(Assembly.GetExecutingAssembly());
            })
            .WithPersistent(settings.ConnectionString);
        services.AddHostedService<TelegramService>();
    }).Build();

await builded.Services.MigrateStateStore();
await builded.RunAsync();