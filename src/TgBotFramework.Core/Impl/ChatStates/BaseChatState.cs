﻿#region Copyright

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
    /// <summary>
    /// Проставляется при создании, в конструктор не выношу чтобы не увеличить количество аргументов у наследников
    /// </summary>
    public IMessenger Messenger { get; set; } = default!;

    public IEventBus EventsBus { get; set; } = default!;

    public ExceptionPolicy? ExceptionPolicy { get; set; }

    public virtual StatePriority Priority => StatePriority.CanBeIgnored;
    public virtual Guid SessionId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Если в этом состоянии впервые, то флажок равен true.
    /// Если состояние обрабатывает сообщение повторно, то false
    /// </summary>
    protected bool IsFirstStateInvoke;

    #region Protected methods

    #region Abstract methods

    protected abstract Task<IStateInfo> InternalProcessMessage(Message receivedMessage);

    #endregion Abstract methods

    protected virtual Task OnStateStartInternal(ChatId chatId)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnStateExitInternal(ChatId chatId)
    {
        return Task.CompletedTask;
    }

    protected virtual async Task PublishError(ChatId processingChatId, Exception ex, string? message)
    {
        EventsBus.Publish(new InStateHandlerError(ex, message ?? string.Empty, this, processingChatId));
        await SendError(processingChatId, ex);
        if (ExceptionPolicy?.CustomActionOnError != null)
        {
            try
            {
                ExceptionPolicy.CustomActionOnError.Invoke(Messenger, processingChatId, ex);
            }
            catch (Exception ex2)
            {
                EventsBus.Publish(new ErrorEvent(ex2, "Ошибка при выполнении доп действия на исключение"));
            }
        }
    }

    protected virtual async Task SendError(ChatId chatId, Exception exception)
    {
        if (ExceptionPolicy == null)
            return;

        if (!ExceptionPolicy.SendToUser)
            return;

        try
        {
            string message = ExceptionPolicy.ExceptionHandler == null
                ? exception.Message
                : ExceptionPolicy.ExceptionHandler(exception);

            if (!string.IsNullOrWhiteSpace(message))
                await Messenger.Send(chatId, new SendInfo(new TextContent(message)));
        }
        catch (Exception ex)
        {
            EventsBus.Publish(new ErrorEvent(ex, "Ошибка при отправке исключения"));
        }
    }

    #endregion Protected methods

    public virtual async Task<IStateInfo> ProcessMessage(Message receivedMessage)
    {
        var baseRet = new StateInfo(this);
        try
        {
            IStateInfo ret = await InternalProcessMessage(receivedMessage);
            return ret;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("message to delete not found"))
            {
                //ничего критичного, сообщение удаляется но ошибка всё равно вылетает :\
                await PublishError(receivedMessage.ChatId, ex, "Ошибка при удалении");
                return baseRet;
            }

            await PublishError(receivedMessage.ChatId, ex, string.Empty);
        }
        finally
        {
            IsFirstStateInvoke = false;
        }

        return baseRet;
    }

    public virtual async Task OnStateStart(ChatId chatId)
    {
        IsFirstStateInvoke = true;
        try
        {
            await OnStateStartInternal(chatId);
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

    public virtual async Task OnStateExit(ChatId chatId)
    {
        try
        {
            await OnStateExitInternal(chatId);
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