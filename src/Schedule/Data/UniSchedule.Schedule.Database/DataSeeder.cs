using Microsoft.EntityFrameworkCore;
using Serilog;
using UniSchedule.Extensions.DI.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Schedule.Database;

public class DataSeeder(DatabaseContext context) : HandbookSeederBase<DatabaseContext>(context)
{
    private static readonly Guid mainGroupId = new("d0b8f7e8-c7a9-4f0a-8b1f-07e5a9a2c1e4");

    /// <inheritdoc cref="HandbookSeederBase{TContext}.SeedAsync" />
    public override async Task SeedAsync()
    {
        await SeedGroupsAsync();
    }

    /// <summary>
    ///     Инициализация групп
    /// </summary>
    private async Task SeedGroupsAsync()
    {
        Log.Information("Инициализация групп...");
        var groupExists = await context.Groups.AnyAsync(g => g.Id == mainGroupId);
        if (!groupExists)
        {
            Log.Information("Создание группы ИВТ-Б21");
            var ivtB21Group = new Group { Id = mainGroupId, Name = "ИВТ-Б21", Grade = 4, HasFixedSubgroups = false };
            await context.Groups.AddAsync(ivtB21Group);
            await context.SaveChangesAsync();
            await InitializeWeeksForGroupAsync(ivtB21Group);
        }
    }

    /// <summary>
    ///     Инициализация недель для указанной группы
    /// </summary>
    /// <param name="group">Группа, для которой инициализируются недели</param>
    private async Task InitializeWeeksForGroupAsync(Group group)
    {
        Log.Information("Инициализация недель для группы {GroupName} (ID: {GroupId})", group.Name, group.Id);

        var weeksToAdd = new List<Week>();

        if (!group.HasFixedSubgroups)
        {
            weeksToAdd.Add(new Week { GroupId = group.Id, Type = WeekType.Even, Subgroup = Subgroup.None });
            weeksToAdd.Add(new Week { GroupId = group.Id, Type = WeekType.Odd, Subgroup = Subgroup.None });
        }
        else
        {
            weeksToAdd.Add(new Week { GroupId = group.Id, Type = WeekType.Even, Subgroup = Subgroup.First });
            weeksToAdd.Add(new Week { GroupId = group.Id, Type = WeekType.Odd, Subgroup = Subgroup.First });
            weeksToAdd.Add(new Week { GroupId = group.Id, Type = WeekType.Even, Subgroup = Subgroup.Second });
            weeksToAdd.Add(new Week { GroupId = group.Id, Type = WeekType.Odd, Subgroup = Subgroup.Second });
        }

        if (weeksToAdd.Count != 0)
        {
            await context.Weeks.AddRangeAsync(weeksToAdd);
            await context.SaveChangesAsync();
            Log.Information("Создано {WeekCount} недель для группы {GroupName}", weeksToAdd.Count, group.Name);

            var daysToAdd = new List<Day>();
            foreach (var week in weeksToAdd)
            {
                Log.Information("Инициализация дней для недели {WeekId} (Тип: {WeekType}, Подгруппа: {Subgroup})",
                    week.Id, week.Type, week.Subgroup);
                daysToAdd.Add(new Day { WeekId = week.Id, DayOfWeek = DayOfWeek.Monday });
                daysToAdd.Add(new Day { WeekId = week.Id, DayOfWeek = DayOfWeek.Tuesday });
                daysToAdd.Add(new Day { WeekId = week.Id, DayOfWeek = DayOfWeek.Wednesday });
                daysToAdd.Add(new Day { WeekId = week.Id, DayOfWeek = DayOfWeek.Thursday });
                daysToAdd.Add(new Day { WeekId = week.Id, DayOfWeek = DayOfWeek.Friday });
                daysToAdd.Add(new Day { WeekId = week.Id, DayOfWeek = DayOfWeek.Saturday });
            }

            if (daysToAdd.Count != 0)
            {
                await context.Days.AddRangeAsync(daysToAdd);
                await context.SaveChangesAsync();
                Log.Information("Создано {DayCount} дней для {ProcessedWeekCount} недель группы {GroupName}",
                    daysToAdd.Count, weeksToAdd.Count, group.Name);
            }
        }
        else
        {
            Log.Warning("Для группы {GroupName} не было создано ни одной недели. Проверьте логику HasFixedSubgroups.",
                group.Name);
        }
    }
}