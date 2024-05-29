#region Copyright

/*
 * File: IStateRepository.cs
 * Author: denisosipenko
 * Created: 2023-12-19
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Persistent;

public interface IStateRepository
{
    Task<ChatStateEntity?> Get(string chatId);
    Task AddOrUpdate(ChatStateEntity newState);
}