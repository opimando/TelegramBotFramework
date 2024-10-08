#region Copyright

/*
 * File: ChatState.cs
 * Author: denisosipenko
 * Created: 2023-11-15
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TgBotFramework.Persistent;

[Index(nameof(ChatId))]
public class ChatStateEntity
{
    [Key] public string ChatId { get; set; } = string.Empty;
    public Guid? SessionId { get; set; }
    public Type? Type { get; set; }
    public object? Argument { get; set; }
}