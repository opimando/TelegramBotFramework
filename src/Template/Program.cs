using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TgBot.Startup;
using TgBotFramework.Core;
#if WithPersistent
using TgBotFramework.Persistent;
#endif

IHost? builded = new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("settings.json", true);
        config.AddJsonFile("logger.json", true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var settings = context.Configuration.Get<Settings>();
        if (settings == null)
            throw new ArgumentNullException(nameof(settings), "Не удалось получить настройки приложения");

        services
            .InitializeBot(settings.TgApiKey, builder =>
            {
                #if WithPersistent
                builder.WithPersistentStore();
                #endif
                builder.WithStates(Assembly.GetExecutingAssembly());
            })
            #if WithPersistent
            .WithPersistent(settings.DbString)
            #endif
            ;

        services.AddHostedService<TelegramHostedService>();
    })
    .UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration))
    .ConfigureLogging((host, config) =>
    {
        if (!host.Configuration.GetChildren().Any(s => s.Key.StartsWith("Serilog")))
            config.AddConsole();
    }).Build();

#if WithPersistent
await builded.Services.MigrateStateStore();
#endif
await builded.RunAsync();