using TgBotFramework.Core;

namespace BotWithPersistenStore.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    public StartHandler(IEventBus eventsBus) : base(eventsBus)
    {
    }

    protected override async Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        var keyboardButtons = new KeyboardButtonGroup(new[]
        {
            new KeyboardButton("Имя 🧘")
        });

        await messenger.Send(receivedMessage.ChatId, new SendInfo(new TextContent(
            "Привет, пользователь :) " +
            "Для ознакомления предлагаю тебе в два этапа заполнить имя и фамилию." +
            "При этом после того как ты заполнишь имя, выключи приложение (контейнер бота) и затем включи его." +
            "Ты увидишь что после заполнения имени состояние (шаг и данные с предыдущего шага) сохранилось и успешно восстановилось." +
            "Затем ты введёшь фамилию и получишь результат."))
        {
            Buttons = keyboardButtons
        });
        return null;
    }
}