using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniSchedule.Abstractions.Helpers;

namespace UniSchedule.Extensions.DI.Sync;

/// <summary>
///     Методы расширения для <see cref="IHost" />
/// </summary>
public static class HostExtensions
{
    /// <summary>
    ///     Синхронизация данных между сервисами
    /// </summary>
    /// <param name="host">Хост</param>
    public static async Task<IHost> SyncDataAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var service = services.GetRequiredService<ISyncService>();
            await service.SyncAsync();
        }

        return host;
    }
}