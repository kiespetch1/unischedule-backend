using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;

namespace UniSchedule.Extensions.DI.Monitoring;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавление мониторинга сервисов в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Обновленная коллекция сервисов</returns>
    public static IServiceCollection AddServiceMonitoring(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddPrometheusExporter()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation());

        return services;
    }
}