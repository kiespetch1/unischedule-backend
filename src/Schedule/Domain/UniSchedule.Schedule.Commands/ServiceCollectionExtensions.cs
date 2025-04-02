using Microsoft.Extensions.DependencyInjection;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Schedule.Commands.Commands;

namespace UniSchedule.Schedule.Commands;

/// <summary>
///     Методы расширения для DI
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddCommands<WeekCommands>();
        services.AddCommands<DayCommands>();
        services.AddCommands<ClassCommands>();
        services.AddCommands<GroupCommands>();
        services.AddCommands<TeacherCommands>();
        services.AddCommands<LocationCommands>();
        services.AddCommands<AnnouncementCommands>();

        return services;
    }
}