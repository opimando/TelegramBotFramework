﻿using Microsoft.Extensions.Hosting;
using TgBotFramework.Core;

namespace BotWithPersistenStore;

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
        _eventsBus.Subscribe<BaseEvent>(evt => Console.WriteLine($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}]: {evt}"));
        _bot.Start();
        await _bot.SetDescription("Я бот, который показываю как работает персистентное хранилище");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _bot.Stop();
        return Task.CompletedTask;
    }
}