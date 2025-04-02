using UniSchedule.Abstractions.Commands;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Enums;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands;

/// <summary>
///     Команды для работы с группами
/// </summary>
public class GroupCommand(DatabaseContext context, ICreateCommand<Week, WeekCreateParameters, Guid> createWeek) :
    ICreateCommand<Group, GroupCreateParameters, Guid>,
    IUpdateCommand<Group, GroupUpdateParameters, Guid>,
    IDeleteCommand<Group, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> ExecuteAsync(
        GroupCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var group = new Group
        {
            Name = parameters.Name,
            Grade = parameters.Grade,
            HasSubgroups = parameters.HasSubgroups,
            HasFixedSubgroups = parameters.HasFixedSubgroups
        };
        await context.Groups.AddAsync(group, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await InitializeWeeksAsync(parameters.HasSubgroups, group.Id, cancellationToken);

        return group.Id;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(
        Guid id,
        GroupUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var group = await context.Groups.SingleOrNotFoundAsync(id, cancellationToken);

        group.Name = parameters.Name;
        group.Grade = parameters.Grade;
        group.HasSubgroups = parameters.HasSubgroups;
        group.HasFixedSubgroups = parameters.HasFixedSubgroups;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var group = await context.Groups.SingleOrNotFoundAsync(id, cancellationToken);

        context.Groups.Remove(group);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task InitializeWeeksAsync(
        bool hasSubgroups,
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var parameters = hasSubgroups switch
        {
            false => new[]
            {
                new WeekCreateParameters { GroupId = groupId, WeekType = WeekType.Even, Subgroup = Subgroup.None },
                new WeekCreateParameters { GroupId = groupId, WeekType = WeekType.Odd, Subgroup = Subgroup.None }
            },
            true => new[]
            {
                new WeekCreateParameters { GroupId = groupId, WeekType = WeekType.Even, Subgroup = Subgroup.First },
                new WeekCreateParameters { GroupId = groupId, WeekType = WeekType.Odd, Subgroup = Subgroup.First },
                new WeekCreateParameters
                {
                    GroupId = groupId, WeekType = WeekType.Even, Subgroup = Subgroup.Second
                },
                new WeekCreateParameters { GroupId = groupId, WeekType = WeekType.Odd, Subgroup = Subgroup.Second }
            }
        };

        foreach (var parameter in parameters)
        {
            await createWeek.ExecuteAsync(parameter, cancellationToken);
        }
    }
}