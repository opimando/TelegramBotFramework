namespace BotWithPersistenStore;

internal class Settings
{
    public string ApiKey { get; set; } = string.Empty;
    public long UserId { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
}