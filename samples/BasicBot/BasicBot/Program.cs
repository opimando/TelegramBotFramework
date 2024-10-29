using System.Reflection;
using BasicBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgBotFramework.Core;

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
            builder.Exceptions.ExceptionHandler =
                exception => $"Пользовательское сообщение о наличии ошибки: {exception.Message}";
        });
        services.AddHostedService<TelegramService>();
    }).Build().RunAsync();