using UniSchedule.Abstractions.Commands;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands;

/// <summary>
///     Команды для работы с днями недели
/// </summary>
public class DayCommands(DatabaseContext context) : ICreateCommand<Day, DayCreateParameters, Guid>
{
    /// <summary>
    ///     Создание дня недели
    /// </summary>
    /// <param name="parameters">Параметры создания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор дня недели</returns>
    public async Task<Guid> ExecuteAsync(DayCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        var day = new Day { DayOfWeek = parameters.DayOfWeek, WeekId = parameters.WeekId };
        context.Days.Add(day);
        await context.SaveChangesAsync(cancellationToken);

        return day.Id;
    }
}