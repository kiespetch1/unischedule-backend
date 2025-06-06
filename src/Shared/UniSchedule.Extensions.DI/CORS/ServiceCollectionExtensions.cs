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
            builder.WithOrigins(
                "https://streaminginfo.ru",
                "https://api.vk.com",
                "http://localhost:3001"
            );
            builder.AllowCredentials();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        }));

        return services;
    }
}