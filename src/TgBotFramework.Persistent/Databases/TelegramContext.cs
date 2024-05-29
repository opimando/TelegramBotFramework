#region Copyright

/*
 * File: TelegramContext.cs
 * Author: denisosipenko
 * Created: 2023-11-15
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TgBotFramework.Persistent;

public class TelegramContext : DbContext
{
    public TelegramContext()
    {
    }

    public TelegramContext(DbContextOptions<TelegramContext> options) : base(options)
    {
    }

    public DbSet<ChatStateEntity> States { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatStateEntity>().Property(p => p.Type).HasConversion(
            body => body == null ? null : JsonConvert.SerializeObject(body),
            json => string.IsNullOrWhiteSpace(json) ? null : JsonConvert.DeserializeObject<Type>(json)
        );
        modelBuilder.Entity<ChatStateEntity>().Property(p => p.Argument).HasConversion(
            body => body == null
                ? null
                : JsonConvert.SerializeObject(body,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All}),
            json => string.IsNullOrWhiteSpace(json)
                ? null
                : JsonConvert.DeserializeObject(json,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All})
        );

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            base.OnConfiguring(optionsBuilder);
            return;
        }

        string connectionString = "Server=localhost;port=5432;Database=tgframework;User Id=postgres;Password=123456";
        optionsBuilder.UseNpgsql(connectionString);
    }
}