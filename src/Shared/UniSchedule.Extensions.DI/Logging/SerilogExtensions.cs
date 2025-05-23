using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;

namespace UniSchedule.Extensions.DI.Logging;

/// <summary>
///     Методы расширения для Serilog
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    ///     Использование конфигурации Serilog
    /// </summary>
    /// <param name="builder">
    ///     <see cref="IHostBuilder" />
    /// </param>
    /// <returns>
    ///     <see cref="IHostBuilder" />
    /// </returns>
    public static IHostBuilder UseSerilogConfiguration(this IHostBuilder builder)
    {
        builder
            .ConfigureUserIdEnricher()
            .UseSerilog((context, provider, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithUserIdEnricher(provider)
                    .Enrich.FromLogContext();
            });

        return builder;
    }

    /// <summary>
    ///     Добавления enricher`а для идентификатора пользователя
    /// </summary>
    /// <param name="configuration">Конфигурация логгера</param>
    /// <param name="serviceProvider">Провайдер сервисов</param>
    private static LoggerConfiguration WithUserIdEnricher(
        this LoggerEnrichmentConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        var enricher = serviceProvider.GetRequiredService<UserIdEnricher>();

        return configuration.With(enricher);
    }

    /// <summary>
    ///     Конфигурация enricher`а для идентификатора пользователя
    /// </summary>
    /// <param name="builder"><see cref="IHostBuilder" />></param>
    private static IHostBuilder ConfigureUserIdEnricher(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<UserIdEnricher>();
        });

        return builder;
    }
}