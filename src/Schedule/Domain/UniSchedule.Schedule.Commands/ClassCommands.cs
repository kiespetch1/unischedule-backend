using UniSchedule.Abstractions.Commands;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands;

/// <summary>
///     Команды для работы с парами
/// </summary>
public class ClassCommands(DatabaseContext context) :
    ICreateCommand<Class, ClassCreateParameters, Guid>,
    IUpdateCommand<Class, ClassUpdateParameters, Guid>,
    IDeleteCommand<Class, Guid>
{
    /// <summary>
    ///     Создание пары
    /// </summary>
    /// <param name="parameters">Параметры создания пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданной пары</returns>
    public async Task<Guid> ExecuteAsync(
        ClassCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var @class = new Class
        {
            DayId = parameters.DayId,
            Name = parameters.Name,
            TeacherId = parameters.TeacherId,
            Type = parameters.Type,
            LocationId = parameters.LocationId,
            Subgroup = parameters.Subgroup,
            WeekType = parameters.WeekType,
            StartedAt = parameters.StartedAt,
            FinishedAt = parameters.FinishedAt,
            IsCancelled = false
        };

        context.Classes.Add(@class);
        await context.SaveChangesAsync(cancellationToken);

        return @class.Id;
    }

    /// <summary>
    ///     Обновление пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="parameters">Параметры обновления пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(
        Guid id,
        ClassUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var @class = await context.Classes.SingleOrNotFoundAsync(id, cancellationToken);

        @class.Name = parameters.Name;
        @class.StartedAt = parameters.StartedAt;
        @class.FinishedAt = parameters.FinishedAt;
        @class.Type = parameters.Type;
        @class.WeekType = parameters.WeekType;
        @class.Subgroup = parameters.Subgroup;
        @class.LocationId = parameters.LocationId;
        @class.TeacherId = parameters.TeacherId;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Удаление пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var @class = await context.Classes.SingleOrNotFoundAsync(id, cancellationToken);

        context.Classes.Remove(@class);
        await context.SaveChangesAsync(cancellationToken);
    }
}