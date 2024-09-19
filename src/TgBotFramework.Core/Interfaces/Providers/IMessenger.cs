#region Copyright

/*
 * File: IMessenger.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Мессенджер
/// </summary>
public interface IMessenger
{
    Task<MessageId> Send(ChatId chatId, SendInfo sendMessageInfo);
    Task<MessageId> Reply(ChatId chatId, MessageId replyTo, SendInfo sendMessageInfo);
    Task Edit(ChatId chatId, MessageId messageToEditId, SendInfo updatedSendMessageInfo);
    Task Delete(ChatId chatId, MessageId messageId);
}