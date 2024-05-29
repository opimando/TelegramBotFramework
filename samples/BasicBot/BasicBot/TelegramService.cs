using Microsoft.Extensions.Hosting;
using TgBotFramework.Core;

namespace BasicBot;

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