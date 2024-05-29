#region Copyright

/*
 * File: ContactContent.cs
 * Author: denisosipenko
 * Created: 2023-08-10
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public class ContactContent : IMessageContent
{
    public ContactContent(UserId? userId, string phoneNumber, string? lastName, string firstName)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public string FirstName { get; }
    public string? LastName { get; }
    public string PhoneNumber { get; }
    public UserId? UserId { get; }

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(FirstName);
    }

    public override string ToString()
    {
        return $"Контакт. Имя {FirstName}, Телефон: {PhoneNumber}";
    }
}