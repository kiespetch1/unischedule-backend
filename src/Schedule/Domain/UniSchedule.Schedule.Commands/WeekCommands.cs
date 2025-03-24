using UniSchedule.Abstractions.Commands;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands;

/// <summary>
///     Команды для работы с неделями
/// </summary>
public class WeekCommands(DatabaseContext context) : ICreateCommand<Week, WeekCreateParameters, Guid>
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
}