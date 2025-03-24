using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Queries;

namespace UniSchedule.Schedule.Queries;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление query-сервисов в DI
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddQuery<WeeksQuery>();

        return services;
    }
}