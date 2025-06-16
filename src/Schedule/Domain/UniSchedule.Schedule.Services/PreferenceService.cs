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
public class PreferenceService(DatabaseContext context) : IPreferenceService
{
    public async Task SetMultipleAsync(
        ScheduleFilteringParameters parameters,
        CancellationToken cancellationToken = default)
    {
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