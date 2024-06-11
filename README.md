# TelegramBotFramework
[![package](https://img.shields.io/badge/TgBotFramework.Core-v0.1.8-blue
)](https://www.nuget.org/packages/TgBotFramework.Core/)

Framework for creating bots with persistent state storage and authorization capabilities
This project targets .NET 6.0

## Getting started

This framework uses Dependency Injection, so:

1. **Create a default console/ASP.NET Core project**
2. **Add the TelegramBotFramework NuGet package**
3. **Add required usings**:
```C#
using TgBotFramework.Core;
using TgBotFramework.Persistent; //if you need to save to a database
```
4. **Register the necessary services**:
Configure the services in your `Startup` class (or equivalent):
```C#
services
  .InitializeBot(settings.ApiKey, builder =>
    {
      builder.WithPersistentStore();
      builder.WithSingleAuthProvider(new UserId(settings.UserId));
      builder.WithStates(Assembly.GetExecutingAssembly());
    })
  .WithPersistent(settings.ConnectionString);
```
5. **Apply database migrations (if using a database)**:
After building the service provider, apply the migrations:
```C#
builded.Services.MigrateStateStore();
```
6. **Start the bot**:
Implement a hosted service to start the bot. For example:
```C#
public class TelegramService : IHostedService
{
    private readonly TelegramBot _bot;
    private readonly IEventBus _eventsBus;

    public TelegramService(TelegramBot bot, IEventBus eventsBus)
    {
        _bot = bot;
        _eventsBus = eventsBus;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _eventsBus.Subscribe<BaseEvent>(evt =>
        {
            if (evt is IStructuredEvent structuredEvent)
            {
                //log structuredEvent like 
                //logger.Log(structuredEvent.Level, structuredEvent.Template, structuredEvent.Items);
            }
            Console.WriteLine($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}]: {evt}");
        });
        _bot.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _bot.Stop();
        return Task.CompletedTask;
    }
}
```

## Samples

Simple examples can be found in the `Samples` folder

## Contributing

Contributions are welcome! Please open an issue or submit a pull request with your improvements.
