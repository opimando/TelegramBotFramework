#region Copyright

/*
 * File: IFileProvider.cs
 * Author: denisosipenko
 * Created: 2024-07-03
 * Copyright © 2024 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public interface IFileProvider
{
    Task<MemoryStream> DownloadFile(string fileId);
}