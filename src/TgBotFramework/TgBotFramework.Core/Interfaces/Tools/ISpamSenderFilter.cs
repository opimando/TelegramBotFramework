#region Copyright

/*
 * File: ISpamSenderFilter.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// Есть ограничение на количество отправляемых сообщений.
/// Необходимо проверять сколько было отправлено в минуту и если что подождать
/// </summary>
public interface ISpamSenderFilter
{
    Task WaitIfLimitHit();
}