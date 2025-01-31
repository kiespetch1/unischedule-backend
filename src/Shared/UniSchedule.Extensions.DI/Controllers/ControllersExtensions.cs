using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace UniSchedule.Extensions.DI.Controllers;

/// <summary>
///     Методы расширения DI для настроек сериализации
/// </summary>
public static class ControllersExtensions
{
    /// <summary>
    ///     Добавление контроллеров с использованием настроек сериализации snake_case
    /// </summary>
    /// <param name="services">
    ///     <see cref="IServiceCollection" />
    /// </param>
    /// <returns>
    ///     <see cref="IServiceCollection" />
    /// </returns>
    public static IServiceCollection AddControllersWithSnakeCase(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.ValueProviderFactories.Add(new SnakeCaseQueryValueProviderFactory());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
                options.SerializerSettings.Converters.Add(new StringEnumConverter(new SnakeCaseNamingStrategy()));
            });

        return services;
    }
}