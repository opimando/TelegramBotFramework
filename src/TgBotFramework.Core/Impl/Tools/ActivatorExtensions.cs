#region Copyright

/*
 * File: ActivatorExtensions.cs
 * Author: denisosipenko
 * Created: 2023-08-13
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using System.Reflection;

namespace TgBotFramework.Core;

public static class ActivatorExtensions
{
    public static T Create<T>(this IServiceProvider serviceProvider) where T : notnull
    {
        List<ConstructorInfo> constructors =
            typeof(T).GetConstructors().OrderByDescending(c => c.GetParameters().Length).ToList();

        foreach (ConstructorInfo constructor in constructors)
            if (serviceProvider.TryCreateWithConstructor(typeof(T), constructor, out T? created))
                return created!;

        throw new Exception($"Не удалось создать экземпляр класса {typeof(T).Name}");
    }

    public static T Create<T>(this IServiceProvider serviceProvider, Type typeToCreate) where T : notnull
    {
        List<ConstructorInfo> constructors =
            typeToCreate.GetConstructors().OrderByDescending(c => c.GetParameters().Length).ToList();

        foreach (ConstructorInfo constructor in constructors)
            if (serviceProvider.TryCreateWithConstructor(typeToCreate, constructor, out T? created))
                return created!;

        throw new Exception($"Не удалось создать экземпляр класса {typeof(T).Name}");
    }

    private static bool TryCreateWithConstructor<T>(this IServiceProvider serviceProvider, Type typeToCreate,
        ConstructorInfo constructor,
        out T? item)
    {
        item = default;
        try
        {
            List<object?> parameters = new();

            foreach (ParameterInfo parameter in constructor.GetParameters())
            {
                if (parameter.ParameterType == typeof(IServiceProvider))
                {
                    parameters.Add(serviceProvider);
                    continue;
                }

                object? serviceEntity = serviceProvider.GetService(parameter.ParameterType);

                if (serviceEntity != null)
                {
                    parameters.Add(serviceEntity);
                    continue;
                }

                if (parameter.IsNullable()) parameters.Add(null);
            }

            if (parameters.Count < constructor.GetParameters().Length)
                return false;

            item = (T) Activator.CreateInstance(typeToCreate, parameters.ToArray())!;
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Не удалось создать экземпляр класса:" + ex.Message);
            return false;
        }
    }

    public static bool IsNullable(this ParameterInfo checkType)
    {
        NullabilityInfo nullabilityInfo = new NullabilityInfoContext().Create(checkType);
        return nullabilityInfo.ReadState is NullabilityState.Nullable;
    }
}