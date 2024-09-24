namespace TgBot.Startup;

public class Settings
{
    public string TgApiKey { get; set; } = string.Empty;
    
    #if WithPersistent
    public string DbString { get; set; } = "";
    #endif
}