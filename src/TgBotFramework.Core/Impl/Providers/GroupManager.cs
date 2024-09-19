#region Copyright

/*
 * File: GroupManager.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgBotFramework.Core;

public class GroupManager : IGroupManager
{
    private readonly ITelegramBotClient _client;
    private readonly IEventBus _eventsBus;

    public GroupManager(ITelegramBotClient client, IEventBus eventsBus)
    {
        _client = client;
        _eventsBus = eventsBus;
    }

    public async Task<bool> IsMemberInGroup(ChatId chatId, UserId userId)
    {
        try
        {
            ChatMember requestUser = await _client.GetChatMemberAsync(chatId.Id, userId);

            return requestUser.Status is ChatMemberStatus.Administrator or ChatMemberStatus.Creator
                or ChatMemberStatus.Member;
        }
        catch (Exception ex)
        {
            _eventsBus.Publish(new ErrorEvent(ex, $"Ошибка при проверка пользователя {userId} в группе {chatId}"));
            return false;
        }
    }

    public async Task KickUserFromGroup(ChatId chatId, UserId userId)
    {
        await _client.BanChatSenderChatAsync(chatId.Id, userId);
        await _client.UnbanChatSenderChatAsync(chatId.Id, userId);
    }
}