#region Copyright

/*
 * File: ChatStateStoreTests.cs
 * Author: denisosipenko
 * Created: 2024-05-28
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

using Moq;
using Xunit;

namespace TgBotFramework.Core.Tests;

public class ChatStateStoreTests
{
    private readonly Mock<IServiceProvider> _serviceProvider = new();
    private readonly Mock<IEventBus> _eventsBus = new();
    private readonly IChatStateStore _store;
    private readonly IStateRegistry _stateRegistry;

    public ChatStateStoreTests()
    {
        _store = new InMemoryStateStore(new ChatStateFactory(_serviceProvider.Object), _eventsBus.Object);
        _stateRegistry = new StateRegistry();
    }

    [Fact]
    public async Task GetNewHandlerForRequest_BasicSearch_ShouldFind()
    {
        _stateRegistry.Add(new[] {typeof(SomeState)});
        _store.SetStates(_stateRegistry.GetRegisteredStates());
        var message = new Message(1, new TextContent("/command"), 1, new User(new UserId(1), "", ""));

        IChatState? state = await _store.GetNewHandlerForRequest(message);

        Assert.NotNull(state);
        Assert.IsType<SomeState>(state);
    }

    [Fact]
    public async Task GetNewHandlerForRequest_CustomSearch_ShouldFind()
    {
        _stateRegistry.Add(new[] {typeof(CustomState)});
        _store.SetStates(_stateRegistry.GetRegisteredStates());
        var message = new Message(1, new TextContent("/command"), 1, new User(new UserId(1), "", ""));
        var imageMessage = new Message(2, new ImageContent(Stream.Null, string.Empty), 1,
            new User(new UserId(1), "", ""));

        IChatState? first = await _store.GetNewHandlerForRequest(message);
        IChatState? second = await _store.GetNewHandlerForRequest(imageMessage);

        Assert.Null(first);
        Assert.NotNull(second);
        Assert.IsType<CustomState>(second);
    }

    [Fact]
    public async Task GetNewHandlerForRequest_EmptyMessage_ShouldThrowArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _store.GetNewHandlerForRequest(default));
    }

    [Fact]
    public async Task PushState_SomeState_ShouldChangeState()
    {
        List<UserStateChangedEvent> changedEvents = new();
        int startedCount = 0;
        _eventsBus.Setup(i => i.Publish(It.IsAny<UserStateChangedEvent>()))
            .Callback((UserStateChangedEvent @event) => { changedEvents.Add(@event); });
        var newState = new SomeState();
        newState.OnAction += type =>
        {
            if (type is ActionType.Start) startedCount++;
        };


        await _store.PushState(1, newState);


        await Task.Delay(TimeSpan.FromMilliseconds(150));
        Assert.Equal(1, startedCount);
        Assert.NotEmpty(changedEvents);
        Assert.Equal(newState.SessionId, changedEvents.First()!.NewState!.SessionId);
    }

    [Fact]
    public async Task PushState_WithCurrentState_ShouldChangeWithExistOldAndStartNewState()
    {
        int exitCount = 0;
        int startedCount = 0;
        List<UserStateChangedEvent> changedEvents = new();
        _eventsBus.Setup(i => i.Publish(It.IsAny<UserStateChangedEvent>()))
            .Callback((UserStateChangedEvent @event) => { changedEvents.Add(@event); });
        var oldState = new SomeState();
        oldState.OnAction += type =>
        {
            if (type is ActionType.Start) startedCount++;
            if (type is ActionType.Exit) exitCount++;
        };
        await _store.PushState(1, oldState);
        var newState = new SomeState();
        newState.OnAction += type =>
        {
            if (type is ActionType.Start) startedCount++;
        };


        await _store.PushState(1, newState);

        await Task.Delay(TimeSpan.FromMilliseconds(150));
        Assert.Equal(2, startedCount);
        Assert.Equal(1, exitCount);
        Assert.NotEmpty(changedEvents);
        Assert.True(changedEvents.Count == 2);
        Assert.Equal(newState.SessionId, changedEvents.Last()!.NewState!.SessionId);
        Assert.Equal(oldState.SessionId, changedEvents.Last()!.OldState!.SessionId);
    }

    internal enum ActionType
    {
        Start,
        Exit,
        Process
    }

    [TelegramState("/command")]
    public class SomeState : IChatState
    {
        public Guid SessionId { get; set; } = Guid.NewGuid();
        internal Action<ActionType>? OnAction { get; set; }

        public Task<IChatState?> ProcessMessage(Message receivedMessage)
        {
            OnAction?.Invoke(ActionType.Process);
            return Task.FromResult((IChatState) null);
        }

        public Task OnStateStart(ChatId chatId)
        {
            OnAction?.Invoke(ActionType.Start);
            return Task.CompletedTask;
        }

        public Task OnStateExit(ChatId chatId)
        {
            OnAction?.Invoke(ActionType.Exit);
            return Task.CompletedTask;
        }

        public StatePriority Priority => StatePriority.CanBeIgnored;
    }

    [TelegramState(true)]
    public class CustomState : SomeState
    {
        [CustomStaticAccessFunction]
        public static bool CanProcess(Message message)
        {
            if (message.Content is ImageContent) return true;
            return false;
        }
    }
}