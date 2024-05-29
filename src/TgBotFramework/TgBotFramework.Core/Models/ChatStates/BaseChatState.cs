#region Copyright

/*
 * File: BaseChatState.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Базовая реализация обработчика
/// </summary>
public abstract class BaseChatState : IChatState
{
    public virtual StatePriority Priority => StatePriority.CanBeIgnored;
    public virtual Guid SessionId { get; set; } = Guid.NewGuid();

    protected readonly IEventBus EventsBus;

    /// <summary>
    /// Если в этом состоянии впервые, то флажок равен true.
    /// Если состояние обрабатывает сообщение повторно, то false
    /// </summary>
    protected bool IsFirstStateInvoke;

    public BaseChatState(IEventBus eventsBus)
    {
        EventsBus = eventsBus;
    }

    #region Protected methods

    #region Abstract methods

    protected abstract Task<IChatState?> InternalProcessMessage(Message receivedMessage, IMessenger messenger);

    #endregion Abstract methods

    protected virtual Task OnStateStartInternal(IMessenger messenger, ChatId chatId)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnStateExitInternal(IMessenger messenger, ChatId chatId)
    {
        return Task.CompletedTask;
    }

    protected virtual Task PublishError(ChatId processingChatId, Exception ex, string? message)
    {
        EventsBus.Publish(new InStateHandlerError(ex, message ?? string.Empty, this, processingChatId));
        return Task.CompletedTask;
    }

    protected virtual async Task SendError(IMessenger messenger, ChatId chatId, string message)
    {
        await messenger.Send(chatId, new SendInfo(new TextContent(message)));
    }

    #endregion Protected methods

    public virtual async Task<IChatState?> ProcessMessage(Message receivedMessage, IMessenger messenger)
    {
        try
        {
            return await InternalProcessMessage(receivedMessage, messenger);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("message to delete not found"))
            {
                //ничего критичного, сообщение удаляется но ошибка всё равно вылетает :\
                await PublishError(receivedMessage.ChatId, ex, "Ошибка при удалении");
                return this;
            }

            await PublishError(receivedMessage.ChatId, ex, string.Empty);
            await SendError(messenger, receivedMessage.ChatId, ex.Message);
        }
        finally
        {
            IsFirstStateInvoke = false;
        }

        return this;
    }

    public virtual async Task OnStateStart(IMessenger messenger, ChatId chatId)
    {
        IsFirstStateInvoke = true;
        try
        {
            await OnStateStartInternal(messenger, chatId);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("message to delete not found"))
            {
                //ничего критичного, сообщение удаляется но ошибка всё равно вылетает :\
                await PublishError(chatId, ex, "Ошибка при удалении");
                return;
            }

            await PublishError(chatId, ex, "Ошибка при старте обработчика");
        }
    }

    public virtual async Task OnStateExit(IMessenger messenger, ChatId chatId)
    {
        try
        {
            await OnStateExitInternal(messenger, chatId);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("message to delete not found"))
            {
                //ничего критичного, сообщение удаляется но ошибка всё равно вылетает :\
                await PublishError(chatId, ex, "Ошибка при удалении");
                return;
            }

            await PublishError(chatId, ex, "Ошибка при завершении обработчика");
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not IChatState state) return false;
        return state.GetType() == GetType() && state.SessionId.Equals(SessionId);
    }

    public override int GetHashCode()
    {
        return SessionId.GetHashCode();
    }
}