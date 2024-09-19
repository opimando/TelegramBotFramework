#region Copyright

/*
 * File: SpamSenderFilter.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class SpamSenderFilter : ISpamSenderFilter
{
    private readonly int _maxMessagesCountInMinute;
    private int _countSendMessagesInThisMinute;
    private DateTime _lastMessageSendMinuteTime = DateTime.Now;

    public SpamSenderFilter(int maxMessagesCountInMinute = 30)
    {
        _maxMessagesCountInMinute = maxMessagesCountInMinute;
    }

    public async Task WaitIfLimitHit()
    {
        if (DateTime.Now.Minute > _lastMessageSendMinuteTime.Minute)
        {
            _countSendMessagesInThisMinute = 1;
            return;
        }

        if (_countSendMessagesInThisMinute > _maxMessagesCountInMinute)
        {
            await Task.Delay(GetWaitTimeToNextMinute());
            _countSendMessagesInThisMinute = 0;
        }

        _countSendMessagesInThisMinute++;
        _lastMessageSendMinuteTime = DateTime.Now;
    }

    private TimeSpan GetWaitTimeToNextMinute()
    {
        DateTime dt = DateTime.Now;
        DateTime next = dt.AddMinutes(1);
        next = new DateTime(next.Year, next.Month, next.Day, next.Hour, next.Minute, 0, DateTimeKind.Local);
        return next - dt;
    }
}