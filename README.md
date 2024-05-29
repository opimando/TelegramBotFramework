# TelegramBotFramework

Framework for creating bots with persistent state storage and authorization capabilities
This project targets .NET 6.0

## Getting started

This framework uses Dependency Injection, so:

1. Create default console/asp.net core project:
2. Add TelegramBotFramework nuget package
3. Add usings (on the top of file):
```
using TgBotFramework.Core;
using TgBotFramework.Persistent; //if need save to db
```
4. Register the necessary features
```
services
  .InitializeBot(settings.ApiKey, builder =>
    {
      builder.WithPersistentStore();
      builder.WithSingleAuthProvider(new UserId(settings.UserId));
      builder.WithStates(Assembly.GetExecutingAssembly());
    })
  .WithPersistent(settings.ConnectionString);
```
5. If you use a database, you need to apply migrations
```
builded.Services.MigrateStateStore();
```
6. To start the bot you need to get TelegramBot and call Start
```
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
                //logger.LogInformation(structuredEvent.Template, structuredEvent.Items);
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

Simple examples can be found in the Samples folder
