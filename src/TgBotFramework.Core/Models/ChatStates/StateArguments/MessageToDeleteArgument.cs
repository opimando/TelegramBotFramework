#region Copyright

/*
 * File: MessageToDeleteArgument.cs
 * Author: denisosipenko
 * Created: 2023-09-02
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Аргумент шага, в котором хранится коллекция сообщения, которые необходимо удалить
/// </summary>
public class MessageToDeleteArgument : StateArgument
{
    public List<MessageId> MessagesIds { get; set; } = new();

    public void Add(MessageId id)
    {
        MessagesIds.Add(id);
    }
}