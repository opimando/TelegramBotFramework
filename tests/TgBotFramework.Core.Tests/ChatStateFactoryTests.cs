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
        StatefulArgument recovered = (state as IChatStateWithData<StatefulArgument>)!.GetData();
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
        StatefulArgument recovered = (state as IChatStateWithData<StatefulArgument>)!.GetData();
        Assert.Equal(argument.SomeInt, recovered.SomeInt);
    }

    [Fact]
    public async Task CreateStateGeneric_WithArgumentsTypeWithoutArgument_Created()
    {
        var state = await _stateFactory.CreateState<StatefulState>();

        Assert.NotNull(state);
        Assert.IsType<StatefulState>(state);
        StatefulArgument recovered = (state as IChatStateWithData<StatefulArgument>)!.GetData();
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
        StatefulArgument recovered = (state as IChatStateWithData<StatefulArgument>)!.GetData();
        Assert.Equal(argument.SomeInt, recovered.SomeInt);
    }

    [Fact]
    public async Task CreateState_WithArgumentsTypeWithoutArgument_Created()
    {
        IChatState state = await _stateFactory.CreateState(typeof(StatefulState));

        Assert.NotNull(state);
        Assert.IsType<StatefulState>(state);
        StatefulArgument recovered = (state as IChatStateWithData<StatefulArgument>)!.GetData();
        Assert.NotNull(recovered);
    }

    [Fact]
    public async Task CreateState_WithSetDefaultServices_Created()
    {
        _serviceProvider.Setup(i => i.GetService(typeof(IMessenger))).Returns(Mock.Of<IMessenger>());
        _serviceProvider.Setup(i => i.GetService(typeof(IEventBus))).Returns(Mock.Of<IEventBus>());

        BaseChatState state = await _stateFactory.CreateState<SomeBaseChildState>();

        Assert.NotNull(state.Messenger);
        Assert.NotNull(state.EventsBus);
    }

    [Fact]
    public async Task CreateState_WithArgs_Created()
    {
        var firstArg = new StatefulArgument {SomeInt = 333};
        var state = await _stateFactory.CreateState<SomeState>(firstArg);

        Assert.NotNull(state);
        Assert.Equal(firstArg.SomeInt, state.GetData().SomeInt);

        StatefulArgument arg = state.GetData();
        var newArg = new SomeExtendedArg {SomeInt = arg.SomeInt};

        var state2 = await _stateFactory.CreateState<WithExtendedArgState>(newArg);

        Assert.NotNull(state);
        Assert.IsType<SomeExtendedArg>(state2.GetData());
        Assert.Equal(newArg.SomeInt, state2.GetData().SomeInt);
        Assert.Equal(newArg.ExtendedString, (state2.GetData() as SomeExtendedArg)!.ExtendedString);
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

        public Task<IStateInfo> ProcessMessage(Message receivedMessage)
        {
            OnAction?.Invoke(ActionType.Process);
            return Task.FromResult((IStateInfo) new StateInfo(null));
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

    public class SomeBaseChildState : BaseChatState
    {
        /// <inheritdoc />
        protected override Task<IStateInfo> InternalProcessMessage(Message receivedMessage)
        {
            throw new NotImplementedException();
        }
    }

    public class StatefulState2 : StatefulState
    {
        public StatefulState2(ISomeInterface service)
        {
        }
    }

    public class AnotherArgument : StateArgument
    {
        public int ZXC { get; set; } = new Random().Next();
    }

    public class WithExtendedArgState : SomeState
    {
        private SomeExtendedArg _extended => (SomeExtendedArg) Argument;

        public override StatefulArgument GetData()
        {
            return _extended;
        }

        /// <inheritdoc />
        public override Task SetData(StatefulArgument data)
        {
            Argument = (SomeExtendedArg) data;
            return Task.CompletedTask;
        }
    }

    public class SomeExtendedArg : StatefulArgument
    {
        public string ExtendedString { get; set; } = new Random().Next().ToString();
    }

    public class SomeState : TestState, IChatStateWithData<StatefulArgument>
    {
        protected StatefulArgument Argument = new();

        /// <inheritdoc />
        public virtual StatefulArgument GetData()
        {
            return Argument;
        }

        /// <inheritdoc />
        public virtual Task SetData(StatefulArgument data)
        {
            Argument = data;
            return Task.CompletedTask;
        }
    }

    public class StatefulState : TestState, IChatStateWithData<StatefulArgument>, IChatStateWithData<AnotherArgument>
    {
        private StatefulArgument _data = new();
        private AnotherArgument _data2 = new();

        StatefulArgument IChatStateWithData<StatefulArgument>.GetData()
        {
            return _data;
        }

        /// <inheritdoc />
        public Task SetData(AnotherArgument data)
        {
            _data2 = data;
            return Task.CompletedTask;
        }

        public Task SetData(StatefulArgument data)
        {
            _data = data;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        AnotherArgument IChatStateWithData<AnotherArgument>.GetData()
        {
            return _data2;
        }
    }
}