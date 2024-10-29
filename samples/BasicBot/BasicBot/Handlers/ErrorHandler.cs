using TgBotFramework.Core;

namespace BasicBot.Handlers;

[TelegramState("/error")]
public class ErrorHandler : BaseChatState, IChatStateWithData<ErrorHandler.ErrorData>
{
    private const string ExitCommand = "!exit";
    private const string HelloMessageText = "Если нужно выйти, нажми на кнопку. А сейчас я буду генерировать ошибки";
    private ErrorData _data = new();

    protected override async Task OnStateStartInternal(ChatId chatId)
    {
        _data.Id = await Messenger.Send(chatId,
            new SendInfo(new TextContent(HelloMessageText))
            {
                Buttons = new InlineButtonGroup(new List<Button> {new InlineButton("Завершить шаг", ExitCommand)})
            }
        );
        throw new Exception("Ошибка в Start методе");
    }

    protected override async Task OnStateExitInternal(ChatId chatId)
    {
        if (_data.Id != null) await Messenger.Edit(chatId, _data.Id, new SendInfo(new TextContent(HelloMessageText)));

        throw new Exception("Ошибка в Exit методе");
    }

    protected override Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        if (receivedMessage.Content is CallbackInlineButtonContent {Data: ExitCommand})
            return Task.FromResult<IStateInfo>(new StateInfo(null));

        throw new Exception("Ошибка основного метода");
    }

    private class ErrorData : StateArgument
    {
        public MessageId? Id { get; set; }
    }

    ErrorData IChatStateWithData<ErrorData>.GetData()
    {
        return _data;
    }

    Task IChatStateWithData<ErrorData>.SetData(ErrorData data)
    {
        _data = data;
        return Task.CompletedTask;
    }
}