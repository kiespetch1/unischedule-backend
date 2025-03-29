using UniSchedule.Abstractions.Commands;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands.Commands;

/// <summary>
///     Команды для работы с неделями
/// </summary>
public class WeekCommands(DatabaseContext context, ICreateCommand<Day, DayCreateParameters, Guid> createDay) :
    ICreateCommand<Week, WeekCreateParameters, Guid>,
    IDeleteCommand<Week, Guid>
{
    /// <summary>
    ///     Создание недели
    /// </summary>
    /// <param name="parameters">Параметры создания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор недели</returns>
    public async Task<Guid> ExecuteAsync(WeekCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        var week = new Week
        {
            WeekType = parameters.WeekType, Subgroup = parameters.Subgroup, GroupId = parameters.GroupId
        };
        context.Weeks.Add(week);
        await context.SaveChangesAsync(cancellationToken);

        return week.Id;
    }

    /// <summary>
    ///     Удаление недели
    /// </summary>
    /// <param name="id">Идентификатор недели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var week = await context.Weeks.SingleOrNotFoundAsync(id, cancellationToken);

        context.Weeks.Remove(week);
        await context.SaveChangesAsync(cancellationToken);
    }
}