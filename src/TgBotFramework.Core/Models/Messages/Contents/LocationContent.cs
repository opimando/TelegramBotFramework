#region Copyright

/*
 * File: LocationContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class LocationContent : IMessageContent
{
    public LocationContent(double latitude, double longitude, int? secondsWillBeOnline)
    {
        Latitude = latitude;
        Longitude = longitude;
        WillBeOnline = secondsWillBeOnline.HasValue ? TimeSpan.FromSeconds(secondsWillBeOnline.Value) : TimeSpan.Zero;
    }

    public double Latitude { get; }
    public double Longitude { get; }
    public TimeSpan WillBeOnline { get; }

    public bool IsEmpty()
    {
        return Latitude <= 0 || Longitude <= 0;
    }

    public override string ToString()
    {
        return $"*Локация*: ш: {Latitude}; д: {Longitude}";
    }
}