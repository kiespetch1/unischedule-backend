using UniSchedule.Extensions.DI.Database;
using UniSchedule.Extensions.DI.Logging;
using UniSchedule.Extensions.DI.Sync;

namespace UniSchedule.Schedule.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.MigrateDatabaseAsync();
        await host.SeedDataAsync();
        await host.SyncDataAsync();

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilogConfiguration()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}