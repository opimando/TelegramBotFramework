#region Copyright

/*
 * File: CustomStaticAccessFunctionAttribute.cs
 * Author: denisosipenko
 * Created: 2023-11-18
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

/// <summary>
/// На метод public static bool SomeName(Message message) можно повесить аттрибут и самому решать подходит ли состояние
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CustomStaticAccessFunctionAttribute : Attribute
{
}