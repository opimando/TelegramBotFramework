#region Copyright

/*
 * File: ChatStateFactoryTests.cs
 * Author: denisosipenko
 * Created: 2023-08-11
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Moq;
using Xunit;

namespace TgBotFramework.Core.Tests;

public class ChatStateFactoryTests
{
    private readonly Mock<IServiceProvider> _serviceProvider = new();
    private readonly IChatStateFactory _stateFactory;

    public ChatStateFactoryTests()
    {
        _stateFactory = new ChatStateFactory(_serviceProvider.Object);
    }

    [Fact]
    public async Task CreateStateGeneric_WithoutArguments_Created()
    {
        var state = await _stateFactory.CreateState<TestState>();

        Assert.NotNull(state);
        Assert.IsType<TestState>(state);
    }

    [Fact]
    public async Task CreateStateGeneric_WithoutArgumentsWithDependencies_Created()
    {
        _serviceProvider.Setup(i => i.GetService(typeof(ISomeInterface))).Returns(Mock.Of<ISomeInterface>());
        var state = await _stateFactory.CreateState<TestState2>();

        Assert.NotNull(state);
        Assert.IsType<TestState2>(state);
    }

    [Fact]
    public async Task CreateStateGeneric_WithoutArgumentsWithoutDependencies_ThrowException()
    {
        await Assert.ThrowsAsync<Exception>(() => _stateFactory.CreateState<TestState2>());
    }

    [Fact]
    public async Task CreateStateGeneric_WithoutArgumentsTypeWithArgument_Created()
    {
        var argument = new StatefulArgument {SomeInt = 123};

        var state = await _stateFactory.CreateState<TestState>(argument);

        Assert.NotNull(state);
        Assert.IsType<TestState>(state);
    }

    [Fact]
    public async Task CreateStateGeneric_WithArgumentsTypeWithArgument_CreatedAndDataRecovered()
    {
        var argument = new StatefulArgument {SomeInt = 123};

        var state = await _stateFactory.CreateState<StatefulState>(argument);

        Assert.NotNull(state);
        Assert.IsType<StatefulState>(state);
        StatefulArgument recovered = state.GetData();
        Assert.Equal(argument.SomeInt, recovered.SomeInt);
    }

    [Fact]
    public async Task CreateStateGeneric_WithArgumentsTypeWithArgumentAndDependencies_CreatedAndDataRecovered()
    {
        _serviceProvider.Setup(i => i.GetService(typeof(ISomeInterface))).Returns(Mock.Of<ISomeInterface>());
        var argument = new StatefulArgument {SomeInt = 123};

        var state = await _stateFactory.CreateState<StatefulState2>(argument);

        Assert.NotNull(state);
        Assert.IsType<StatefulState2>(state);
        StatefulArgument recovered = state.GetData();
        Assert.Equal(argument.SomeInt, recovered.SomeInt);
    }

    [Fact]
    public async Task CreateStateGeneric_WithArgumentsTypeWithoutArgument_Created()
    {
        var state = await _stateFactory.CreateState<StatefulState>();

        Assert.NotNull(state);
        Assert.IsType<StatefulState>(state);
        StatefulArgument recovered = state.GetData();
        Assert.NotNull(recovered);
    }

    [Fact]
    public async Task CreateState_WithoutArguments_Created()
    {
        IChatState state = await _stateFactory.CreateState(typeof(TestState));

        Assert.NotNull(state);
        Assert.IsType<TestState>(state);
    }

    [Fact]
    public async Task CreateState_WithoutArgumentsTypeWithArgument_Created()
    {
        var argument = new StatefulArgument {SomeInt = 123};

        IChatState state = await _stateFactory.CreateState(typeof(TestState), argument);

        Assert.NotNull(state);
        Assert.IsType<TestState>(state);
    }

    [Fact]
    public async Task CreateState_WithArgumentsTypeWithArgument_CreatedAndDataRecovered()
    {
        var argument = new StatefulArgument {SomeInt = 123};

        IChatState state = await _stateFactory.CreateState(typeof(StatefulState), argument);

        Assert.NotNull(state);
        Assert.IsType<StatefulState>(state);
        StatefulArgument recovered = (state as StatefulState).GetData();
        Assert.Equal(argument.SomeInt, recovered.SomeInt);
    }

    [Fact]
    public async Task CreateState_WithArgumentsTypeWithoutArgument_Created()
    {
        IChatState state = await _stateFactory.CreateState(typeof(StatefulState));

        Assert.NotNull(state);
        Assert.IsType<StatefulState>(state);
        StatefulArgument recovered = (state as StatefulState).GetData();
        Assert.NotNull(recovered);
    }

    internal enum ActionType
    {
        Start,
        Exit,
        Process
    }

    public class TestState : IChatState
    {
        public Guid SessionId { get; set; } = Guid.NewGuid();
        internal Action<ActionType>? OnAction { get; set; }

        public Task<IChatState?> ProcessMessage(Message receivedMessage, IMessenger messenger)
        {
            OnAction?.Invoke(ActionType.Process);
            return Task.FromResult((IChatState) null);
        }

        public Task OnStateStart(IMessenger messenger, ChatId chatId)
        {
            OnAction?.Invoke(ActionType.Start);
            return Task.CompletedTask;
        }

        public Task OnStateExit(IMessenger messenger, ChatId chatId)
        {
            OnAction?.Invoke(ActionType.Exit);
            return Task.CompletedTask;
        }

        public StatePriority Priority => StatePriority.CanBeIgnored;
    }

    public class StatefulArgument : StateArgument
    {
        public int SomeInt { get; set; }
    }

    public interface ISomeInterface
    {
    }

    public class TestState2 : TestState
    {
        public TestState2(ISomeInterface service)
        {
        }
    }

    public class StatefulState2 : StatefulState
    {
        public StatefulState2(ISomeInterface service)
        {
        }
    }

    public class StatefulState : TestState, IChatStateWithData<StatefulArgument>
    {
        private StatefulArgument _data = new();

        public StatefulArgument GetData()
        {
            return _data;
        }

        public Task SetData(StatefulArgument data)
        {
            _data = data;
            return Task.CompletedTask;
        }
    }
}