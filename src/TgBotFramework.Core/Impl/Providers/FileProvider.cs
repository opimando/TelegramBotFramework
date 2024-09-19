#region Copyright

/*
 * File: FileProvider.cs
 * Author: denisosipenko
 * Created: 2024-07-03
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

using Telegram.Bot;

namespace TgBotFramework.Core;

public class FileProvider : IFileProvider
{
    private readonly ITelegramBotClient _client;

    public FileProvider(ITelegramBotClient client)
    {
        _client = client;
    }

    public async Task<MemoryStream> DownloadFile(string fileId)
    {
        MemoryStream stream = new();
        await _client.GetInfoAndDownloadFileAsync(fileId, stream);

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
}