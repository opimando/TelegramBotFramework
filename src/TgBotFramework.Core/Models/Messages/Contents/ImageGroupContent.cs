#region Copyright

/*
 * File: ImageGroupContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ImageGroupContent : IMessageContent
{
    public ImageGroupContent(string? imageGroupId, IEnumerable<ImageContent> photos, string? caption = null)
    {
        Images = photos.ToList();
        ImageGroupId = imageGroupId;
        Caption = caption;
    }

    public List<ImageContent> Images { get; }
    public string? ImageGroupId { get; }
    public string? Caption { get; }

    public bool IsEmpty()
    {
        return Images.Count == 0;
    }

    public override string ToString()
    {
        return $"*Коллекция изображений*: {string.Join(";", Images.Select(i => i.ToString()))}";
    }
}