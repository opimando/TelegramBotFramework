namespace TgBotFramework.Core;

public class ExceptionPolicy
{
    /// <summary>
    /// Нужно ли отправлять сообщение пользователю, если в ходе обработки сообщения произошла ошибка
    /// </summary>
    public bool SendToUser { get; set; } = true;

    /// <summary>
    /// Функция, обрабатывающая ошибку и возвращающая текст, который будет отправляться пользователю.
    /// Если возвращается пустая строка, то отправки не будет.
    /// </summary>
    public Func<Exception, string>? ExceptionHandler { get; set; } = DefaultErrorMessage;

    /// <summary>
    /// Пользовательское дополнительное событие при обработке ошибки. Не зависит от SendToUser и выполняется после уведомления пользователя.
    /// </summary>
    public Action<IMessenger, ChatId, Exception>? CustomActionOnError { get; set; }

    public static readonly Func<Exception, string> DefaultHiddenMessage = _ => "Произошла ошибка";

    public static readonly Func<Exception, string> DefaultErrorMessage =
        exception => $"Произошла ошибка: {exception.Message}";
}