#region Copyright

/*
 * File: MessageQueueTests.cs
 * Author: denisosipenko
 * Created: 2024-05-28
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

using Moq;
using Xunit;

namespace TgBotFramework.Core.Tests;

public class MessageProcessQueueTests
{
    private readonly Mock<IMessageProcessor> _messageProcessor = new();
    private readonly IMessageProcessQueue _queue;

    public MessageProcessQueueTests()
    {
        _queue = new MessageProcessQueue(_messageProcessor.Object);
    }

    [Fact]
    public async Task Enqueue_BasicMessage_ShouldProcess()
    {
        int processedCount = 0;
        _messageProcessor.Setup(i => i.Process(It.IsAny<Message>()))
            .Callback(() => processedCount++);

        _queue.Enqueue(123, new Message(1, new TextContent("asd"), 123,
            new User(new UserId(123), "", "")));

        await Task.Delay(TimeSpan.FromMilliseconds(500));
        Assert.Equal(1, processedCount);
    }

    [Fact]
    public async Task Enqueue_ParallelSend_ShouldProcessInQueue()
    {
        int processedCount = 0;
        _messageProcessor.Setup(i => i.Process(It.IsAny<Message>()))
            .Returns(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(300));
                processedCount++;
            });

        Parallel.ForEach(Enumerable.Range(0, 5), _ =>
        {
            _queue.Enqueue(123, new Message(1, new TextContent("asd"), 123,
                new User(new UserId(123), "", "")));
        });

        await Task.Delay(TimeSpan.FromMilliseconds(650));
        Assert.True(processedCount is > 1 and < 5);
        await Task.Delay(TimeSpan.FromMilliseconds(1500));
        Assert.Equal(5, processedCount);
    }
}