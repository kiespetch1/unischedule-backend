using Microsoft.Extensions.DependencyInjection;

namespace UniSchedule.Extensions.DI.CORS;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление CORS в DI
    /// </summary>
    /// <param name="corsPolicyName">Название политики CORS</param>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddCors(this IServiceCollection services, string corsPolicyName)
    {
        services.AddCors(options => options.AddPolicy(corsPolicyName, builder =>
        {
            // TODO: подумать в будущем над cors
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        }));

        return services;
    }
}