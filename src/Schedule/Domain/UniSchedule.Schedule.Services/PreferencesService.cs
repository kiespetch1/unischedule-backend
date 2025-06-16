using Microsoft.EntityFrameworkCore;
using UniSchedule.Extensions.Collections;
using UniSchedule.Extensions.Exceptions;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Services.Abstractions;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Services;

/// <summary>
///     Сервис для работы с персональной фильтрацией
/// </summary>
public class PreferencesService(DatabaseContext context) : IPreferencesService
{
    public async Task SetMultipleAsync(
        ScheduleFilteringParameters parameters,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var ids = context.FilteringInfo
            .Where(p => p.CreatedBy == userId)
            .Select(x => x.CreatedBy)
            .ToList();
        await context.FilteringInfo
            .Where(x => ids.Contains(x.CreatedBy!.Value))
            .ExecuteDeleteAsync(cancellationToken);

        foreach (var parameter in parameters.FilteringParameters)
        {
            context.FilteringInfo.Add(new ScheduleFilteringOption
            {
                ClassName = parameter.ClassName, Subgroup = parameter.Subgroup
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var preference = await context.FilteringInfo
            .SingleOrNotFoundAsync(id, cancellationToken);
        if (preference.CreatedBy != userId)
        {
            throw new NoAccessRightsException("Вы не можете удалить чужое предпочтение");
        }

        context.FilteringInfo.Remove(preference);
        await context.SaveChangesAsync(cancellationToken);
    }
}