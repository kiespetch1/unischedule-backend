using UniSchedule.Abstractions.Commands;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands.Commands;

/// <summary>
///     Команды для работы с местами проведения
/// </summary>
public class LocationCommands(DatabaseContext context) :
    ICreateCommand<Location, LocationCreateParameters, Guid>,
    IUpdateCommand<Location, LocationUpdateParameters, Guid>,
    IDeleteCommand<Location, Guid>
{
    /// <summary>
    ///     Создание места проведения
    /// </summary>
    /// <param name="parameters">Параметры создания места проведения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданного места проведения</returns>
    public async Task<Guid> ExecuteAsync(LocationCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var location = new Location
        {
            Name = parameters.Name, Link = parameters.Link, LocationType = parameters.LocationType
        };

        context.Locations.Add(location);
        await context.SaveChangesAsync(cancellationToken);

        return location.Id;
    }

    /// <summary>
    ///     Обновление места проведения
    /// </summary>
    /// <param name="id">Идентификатор места проведения</param>
    /// <param name="parameters">Параметры обновления места проведения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(Guid id, LocationUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var location = await context.Locations.SingleOrNotFoundAsync(id, cancellationToken);

        location.Name = parameters.Name;
        location.Link = parameters.Link;
        location.LocationType = parameters.LocationType;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Удаление места проведения
    /// </summary>
    /// <param name="id">Идентификатор места проведения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var location = await context.Locations.SingleOrNotFoundAsync(id, cancellationToken);

        context.Locations.Remove(location);
        await context.SaveChangesAsync(cancellationToken);
    }
}