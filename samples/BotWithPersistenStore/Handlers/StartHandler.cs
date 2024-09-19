using TgBotFramework.Core;

namespace BotWithPersistenStore.Handlers;

[TelegramState("/start")]
public class StartHandler : BaseChatState
{
    protected override async Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
    {
        var keyboardButtons = new KeyboardButtonGroup(new[]
        {
            new KeyboardButton("Имя 🧘")
        });

        await Messenger.Send(receivedMessage.ChatId, new SendInfo(new TextContent(
            "Привет, пользователь :) " +
            "Для ознакомления предлагаю тебе в два этапа заполнить имя и фамилию." +
            "При этом после того как ты заполнишь имя, выключи приложение (контейнер бота) и затем включи его." +
            "Ты увидишь что после заполнения имени состояние (шаг и данные с предыдущего шага) сохранилось и успешно восстановилось." +
            "Затем ты введёшь фамилию и получишь результат."))
        {
            Buttons = keyboardButtons
        });
        return new StateInfo(null);
    }
}