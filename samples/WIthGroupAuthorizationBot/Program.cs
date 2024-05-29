using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgBotFramework.Core;
using WithGroupAuthorizationBot;

await new HostBuilder()
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

        services.InitializeBot(settings.ApiKey, builder =>
        {
            builder.WithStates(Assembly.GetExecutingAssembly());
            builder.WithGroupAuthProvider(settings.GroupId);
        });
        services.AddHostedService<TelegramService>();
    }).Build().RunAsync();