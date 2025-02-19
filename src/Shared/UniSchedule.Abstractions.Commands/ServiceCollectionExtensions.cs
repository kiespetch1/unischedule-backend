using Microsoft.Extensions.DependencyInjection;

namespace UniSchedule.Abstractions.Commands;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Подключение Command-слоя в DI
    /// </summary>
    /// <typeparam name="TImplementation">Реализация command-сервиса</typeparam>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddCommands<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        var interfaceTypes = typeof(TImplementation)
            .GetInterfaces()
            .Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(ICreateCommand<,,>)
                                            || x.GetGenericTypeDefinition() == typeof(IUpdateCommand<,,>)
                                            || x.GetGenericTypeDefinition() == typeof(IDeleteCommand<,>)));
        foreach (var interfaceType in interfaceTypes)
        {
            services.AddScoped(interfaceType, typeof(TImplementation));
        }

        return services;
    }
}