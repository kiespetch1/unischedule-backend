using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Queries.Base;

namespace UniSchedule.Abstractions.Queries;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Подключение Query-слоя в DI
    /// </summary>
    /// <typeparam name="TImplementation">Реализация query-сервиса</typeparam>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddQuery<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        var interfaceTypes = typeof(TImplementation)
            .GetInterfaces()
            .Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(ISingleQuery<,>)
                                            || x.GetGenericTypeDefinition() == typeof(ISingleQueryByPredicate<,>)
                                            || x.GetGenericTypeDefinition() == typeof(IMultipleQuery<,>)
                                            || x.GetGenericTypeDefinition() == typeof(IMultipleQueryByPredicate<,>)));
        foreach (var interfaceType in interfaceTypes)
        {
            services.AddScoped(interfaceType, typeof(TImplementation));
        }

        return services;
    }
}