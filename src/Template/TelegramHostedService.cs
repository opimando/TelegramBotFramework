using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TgBotFramework.Core;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace TgBot.Startup;

public class TelegramHostedService : IHostedService
{
    private readonly TelegramBot _bot;
    private readonly IEventBus _eventsBus;
    private readonly ILogger<TelegramHostedService> _logger;

    public TelegramHostedService(TelegramBot bot, IEventBus eventsBus, ILogger<TelegramHostedService> logger)
    {
        _bot = bot;
        _eventsBus = eventsBus;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _eventsBus.Subscribe<BaseEvent>(@event =>
        {
            if (@event is IStructuredEvent structuredEvent)
                _logger.Log(GetLevel(structuredEvent.Level), structuredEvent.Template, structuredEvent.Items);
            else
                _logger.LogInformation(@event.ToString());
        });
        _bot.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _bot.Stop();
        return Task.CompletedTask;
    }

    private LogLevel GetLevel(TgBotFramework.Core.LogLevel level)
    {
        return level switch
        {
            TgBotFramework.Core.LogLevel.Trace => LogLevel.Trace,
            TgBotFramework.Core.LogLevel.Information => LogLevel.Information,
            TgBotFramework.Core.LogLevel.Warning => LogLevel.Warning,
            TgBotFramework.Core.LogLevel.Error => LogLevel.Error,
            TgBotFramework.Core.LogLevel.Critical => LogLevel.Critical,
            _ => LogLevel.Debug
        };
    }
}