#region Copyright

/*
 * File: StateRepository.cs
 * Author: denisosipenko
 * Created: 2023-11-15
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TgBotFramework.Persistent;

public class StateRepository : IStateRepository
{
    private readonly IDbContextFactory<TelegramContext> _dbContextFactory;

    public StateRepository(IDbContextFactory<TelegramContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ChatStateEntity?> Get(string chatId)
    {
        await using TelegramContext context = await _dbContextFactory.CreateDbContextAsync();
        return await context.States.FirstOrDefaultAsync(state => state.ChatId == chatId);
    }

    public async Task AddOrUpdate(ChatStateEntity newState)
    {
        ChatStateEntity? baseState = await Get(newState.ChatId);
        await using TelegramContext context = await _dbContextFactory.CreateDbContextAsync();

        EntityEntry<ChatStateEntity> entity = context.Entry(newState);
        entity.State = baseState == null ? EntityState.Added : EntityState.Modified;
        await context.SaveChangesAsync();
    }
}