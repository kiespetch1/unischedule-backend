using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace UniSchedule.Validation;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление валидации в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssemblies(Assembly
            .GetEntryAssembly()?
            .GetReferencedAssemblies()
            .Select(Assembly.Load));

        return services;
    }

    /// <summary>
    ///     Подключение валидатора в DI
    /// </summary>
    /// <typeparam name="TImplementation">Реализация валидатора</typeparam>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddValidator<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        var interfaceTypes = typeof(TImplementation)
            .GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityValidator<,>));
        foreach (var interfaceType in interfaceTypes)
        {
            services.AddScoped(interfaceType, typeof(TImplementation));
        }

        return services;
    }
}