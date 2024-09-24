#region Copyright

/*
 * File: MessageProcessor.cs
 * Author: denisosipenko
 * Created: 2023-08-11
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class MessageProcessor : IMessageProcessor
{
    private readonly IChatStateStore _stateStore;
    private readonly IMessenger _messenger;
    private readonly IEventBus _eventsBus;
    private readonly List<IMessageProcessMiddleware> _middlewares;

    public MessageProcessor(
        IChatStateStore stateStore,
        IMessenger messenger,
        IEventBus eventsBus,
        IEnumerable<IMessageProcessMiddleware> middlewares)
    {
        _stateStore = stateStore;
        _messenger = messenger;
        _eventsBus = eventsBus;
        _middlewares = middlewares.ToList();
    }

    public async Task Process(Message message)
    {
        try
        {
            IChatState? currentState = await _stateStore.GetChatState(message.ChatId);
            IChatState? handlerToProcess = await _stateStore.GetNewHandlerForRequest(message);

            if (handlerToProcess == null && currentState == null)
            {
                await ReplyUnknownMessage(message.ChatId);
                return;
            }

            IStateInfo nextState = await ExecuteWithMiddlewares(message, handlerToProcess, currentState);

            message.ProcessedTime = DateTime.Now;
            await _stateStore.SaveState(message.ChatId, nextState.NextState);
            await InvokeAgainIfStateInfo(nextState, message);
        }
        catch (Exception ex)
        {
            _eventsBus.Publish(new ErrorMessageProcessingEvent(ex, message));
        }
    }

    private async Task InvokeAgainIfStateInfo(IStateInfo stateInfo, Message message)
    {
        if (stateInfo.NextState == null || stateInfo.ExecutionType == ExecutionType.NextInvoke)
            return;

        IStateInfo next = await ExecuteWithMiddlewares(message, stateInfo.NextState, stateInfo.NextState);
        await _stateStore.SaveState(message.ChatId, next.NextState);
    }

    private async Task<IStateInfo> ProcessInternal(Message message, IChatState executingState, IChatState? oldState)
    {
        try
        {
            if (!executingState.Equals(oldState))
                await _stateStore.NotifyReplacedStates(message.ChatId, oldState, executingState);

            IStateInfo nextState = await Process(executingState, message);
            if (!executingState.Equals(nextState.NextState))
                await _stateStore.NotifyReplacedStates(message.ChatId, executingState, nextState.NextState);

            return nextState;
        }
        catch (Exception ex)
        {
            _eventsBus.Publish(new ErrorMessageProcessingEvent(ex, message));
            return new StateInfo(executingState);
        }
    }

    public async Task<IStateInfo> ExecuteWithMiddlewares(Message message, IChatState? state, IChatState? oldState)
    {
        IChatState? executingState = state ?? oldState;

        if (executingState == null) throw new ArgumentException("Не определён обработчик сообщения");
        if (!_middlewares.Any()) return await ProcessInternal(message, executingState, oldState);

        var context = new MessageExecutionContext(message) {ExecutingState = executingState};

        var processorWrapper = new ExecuteProcessorWrapper(async () =>
        {
            if (context.ExecutingState == null) return new StateInfo(null);

            return await ProcessInternal(context.Message, context.ExecutingState, oldState);
        });

        MiddlewareDelegate pipeline = () => processorWrapper.Process(context, () => Task.CompletedTask);
        foreach (IMessageProcessMiddleware ware in _middlewares)
        {
            MiddlewareDelegate pipeline1 = pipeline;
            pipeline = () => ware.Process(context, pipeline1);
        }

        await pipeline.Invoke();
        return context.Result;
    }

    private async Task<IStateInfo> Process(IChatState? state, Message message)
    {
        try
        {
            if (state == null)
                return new StateInfo(null);

            return await state.ProcessMessage(message);
        }
        catch (Exception ex)
        {
            _eventsBus.Publish(new ErrorMessageProcessingEvent(ex, message));
            return new StateInfo(state);
        }
    }

    private Task ReplyUnknownMessage(ChatId chatId)
    {
        return _messenger.Send(chatId, new SendInfo(new TextContent("Не удалось распознать команду")));
    }
}