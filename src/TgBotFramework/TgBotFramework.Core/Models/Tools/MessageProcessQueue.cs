#region Copyright

/*
 * File: MessageProcessQueue.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Collections.Concurrent;

namespace TgBotFramework.Core;

public class MessageProcessQueue : IMessageProcessQueue
{
    private readonly IMessageProcessor _messageProcessor;

    private readonly ConcurrentDictionary<ChatId, UserMessagesQueue> _userMessages = new();

    public MessageProcessQueue(IMessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    public void Enqueue(ChatId chatId, Message newMessage)
    {
        UserMessagesQueue queueToProcess = new();

        _userMessages.AddOrUpdate(chatId,
            key =>
            {
                queueToProcess.Enqueue(newMessage);
                return queueToProcess;
            },
            (key, exist) =>
            {
                exist.Enqueue(newMessage);
                queueToProcess = exist;
                return exist;
            });

        Task.Run(() => Execute(chatId, queueToProcess!));
    }

    private async Task Execute(ChatId chatId, UserMessagesQueue queue)
    {
        await queue.ExecutorLock.WaitAsync();

        try
        {
            if (!queue.TryDequeue(out Message? message))
            {
                TryRemoveUser(chatId);
                return;
            }

            await _messageProcessor.Process(message);
            if (queue.Count == 0) TryRemoveUser(chatId);
        }
        finally
        {
            queue.ExecutorLock.Release();
        }
    }

    private void TryRemoveUser(ChatId chatId)
    {
        _userMessages.TryRemove(chatId, out _);
    }
}