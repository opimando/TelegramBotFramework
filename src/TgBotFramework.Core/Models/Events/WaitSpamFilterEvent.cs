namespace TgBotFramework.Core;

public class WaitSpamFilterEvent : BaseEvent, IStructuredEvent
{
    public LogLevel Level => LogLevel.Warning;
    public string Template => "Ждём спам фильтр для отправки следующего сообщения";
    public object?[] Items => Array.Empty<object?>();
}