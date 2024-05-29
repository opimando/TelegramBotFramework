#region Copyright

/*
 * File: SelectableMessage.cs
 * Author: denisosipenko
 * Created: 2023-09-05
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Сообщение с Inline кнопками, которые могут выбираться и менять своё состояние
/// </summary>
public class MessageWithSelectableItems
{
    public MessageWithSelectableItems(IMessageContent content)
    {
        Content = content;
    }

    public IMessageContent Content { get; set; }
    public Func<MessageWithSelectableItems, InlineButtonGroup>? CustomCreateFunction { get; set; }
    public MessageId? MessageId { get; set; }
    public List<SelectableInlineItem> Items { get; set; } = new();

    public void UpdateSelect(string selectedItemId)
    {
        SelectableInlineItem? item = Items.FirstOrDefault(i => i.Id == selectedItemId);
        if (item == null) return;
        item.IsSelected = !item.IsSelected;
    }

    public async Task AddOrUpdate(IMessenger messenger, ChatId chatId)
    {
        InlineButtonGroup buttons;
        if (CustomCreateFunction == null)
            buttons = new InlineButtonGroup(new List<List<InlineButton>>(Items.Select(s => new List<InlineButton>
                {new(s.ToString(), s.Id)})));
        else
            buttons = CustomCreateFunction(this);

        if (MessageId == null)
            MessageId = await messenger.Send(chatId, new SendInfo(Content)
            {
                Buttons = buttons
            });
        else
            await messenger.Edit(chatId, MessageId, new SendInfo(Content) {Buttons = buttons});
    }
}