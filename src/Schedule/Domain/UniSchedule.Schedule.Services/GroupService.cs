using System.Text.RegularExpressions;
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Entities.Enums;
using UniSchedule.Schedule.Services.Abstractions;
using UniSchedule.Shared.DTO.Models;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Services;

/// <summary>
///     Сервис для работы с группами
/// </summary>
public partial class GroupService(DatabaseContext context, IBrowsingContext browsingContext) : IGroupService
{
    /// <inheritdoc />
    public async Task PromoteGroupsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await context.Groups
            .Where(x => x.Grade < 5)
            .ToListAsync(cancellationToken);

        foreach (var group in groups)
        {
            group.Grade++;
        }

        await context.FilteringInfo.ExecuteDeleteAsync(cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ImportClassesScheduleAsync(
        ClassScheduleImportParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var group = await context.Groups
            .Include(g => g.Weeks)
            .ThenInclude(w => w.Days)
            .ThenInclude(d => d.Classes)
            .SingleOrNotFoundAsync(parameters.GroupId, cancellationToken);

        foreach (var day in group.Weeks.SelectMany(w => w.Days))
        {
            day.Classes.Clear();
        }

        var parsedDays = await ParseWeeksAsync(parameters.Url, cancellationToken);

        var weeks = group.Weeks.ToDictionary(x => (x.Type, x.Subgroup));

        foreach (var parsedDay in parsedDays)
        {
            foreach (var parsedClass in parsedDay.Classes)
            {
                IEnumerable<Week> targetWeeks;

                if (!group.HasFixedSubgroups)
                {
                    targetWeeks = parsedClass.WeekType switch
                    {
                        WeekType.Every => new[]
                        {
                            weeks[(WeekType.Even, Subgroup.None)], weeks[(WeekType.Odd, Subgroup.None)]
                        },
                        WeekType.Even => new[] { weeks[(WeekType.Even, Subgroup.None)] },
                        WeekType.Odd => new[] { weeks[(WeekType.Odd, Subgroup.None)] },
                        _ => Array.Empty<Week>()
                    };
                }
                else
                {
                    targetWeeks = parsedClass.Subgroup switch
                    {
                        Subgroup.None => parsedClass.WeekType switch
                        {
                            WeekType.Every => weeks.Values,
                            WeekType.Even => weeks.Values.Where(w => w.Type == WeekType.Even),
                            WeekType.Odd => weeks.Values.Where(w => w.Type == WeekType.Odd),
                            _ => []
                        },
                        _ => parsedClass.WeekType switch
                        {
                            WeekType.Every => new[]
                            {
                                weeks[(WeekType.Even, parsedClass.Subgroup)],
                                weeks[(WeekType.Odd, parsedClass.Subgroup)]
                            },
                            _ => new[] { weeks[(parsedClass.WeekType, parsedClass.Subgroup)] }
                        }
                    };
                }

                foreach (var week in targetWeeks)
                {
                    var day = week.Days.Single(d => d.DayOfWeek == parsedDay.DayOfWeek);

                    var entity = new Class
                    {
                        Name = parsedClass.Name,
                        StartedAt = parsedClass.StartedAt,
                        FinishedAt = parsedClass.FinishedAt,
                        Type = parsedClass.Type,
                        WeekType = parsedClass.WeekType,
                        Subgroup = parsedClass.Subgroup,
                        IsCancelled = false,
                        LocationId = parsedClass.LocationId,
                        TeacherId = parsedClass.TeacherId
                    };

                    context.Classes.Add(entity);
                    day.Classes.Add(entity);
                }
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<DayParseModel>> ParseWeeksAsync(string url, CancellationToken cancellationToken = default)
    {
        const string firstSubgroupSuffix = "- 1 п/г";
        const string secondSubgroupSuffix = "- 2 п/г";

        Log.Debug("Начало парсинга по ссылке {Url}", url);
        var document = await browsingContext.OpenAsync(url, cancellationToken);
        Log.Debug("Страница загружена. Длина документа - {DocumentLength} символов", document.Body!.TextContent.Length);

        var dayElements = document.QuerySelectorAll(".card-body.p-0").Skip(1).Take(6).ToList();
        var days = new List<DayParseModel>();
        Log.Debug("Найдено {DaysCount} дней", dayElements.Count);

        foreach (var dayItem in dayElements)
        {
            var dayNameElement = dayItem.QuerySelector(".card-title strong");
            var dayName = dayNameElement?.TextContent.Trim();

            if (dayName == null)
            {
                Log.Error("Не удалось получить название дня. Пропускаем день...");
                continue;
            }

            Log.Debug("Обработка дня {DayName}", dayName);

            var dayOfWeek = ParseDayOfWeek(dayName.ToUpperInvariant());

            if (dayOfWeek == null)
            {
                Log.Error("Неизвестное название дня: {DayName}. Пропускаем день...", dayName);
                continue;
            }

            var classElements = dayItem.QuerySelectorAll(".col-md-12.py-1.lesson");
            var classes = new List<ClassParseModel>();

            foreach (var classItem in classElements)
            {
                var timeElement =
                    classItem.QuerySelector(".text-sm-left.text-center.pr-0.col-12.col-sm-3.col-md-2.col-lg-2");
                var timeRange = timeElement?.TextContent.Trim().Split('—');
                TimeOnly startTime, endTime;

                if (timeRange?.Length == 2)
                {
                    startTime = TimeOnly.Parse(timeRange[0]);
                    endTime = TimeOnly.Parse(timeRange[1]);
                }
                else
                {
                    Log.Error("Неверный формат времени: {TimeRange}. Пропускаем пару...", timeRange);
                    continue;
                }

                var classRows = classItem.QuerySelectorAll(".row");
                foreach (var row in classRows)
                {
                    var @class = new ClassParseModel { StartedAt = startTime, FinishedAt = endTime };

                    var lessonTypeElement = row.QuerySelector(".lesson-type");
                    if (lessonTypeElement == null)
                    {
                        Log.Error("Не удалось получить тип пары: {LessonTypeElement}. Пропускаем пару...",
                            lessonTypeElement);
                        continue;
                    }

                    var lessonType = ParseClassType(lessonTypeElement.TextContent.Trim());
                    @class.Type = lessonType;

                    var periodicityElement = row.QuerySelector(".lesson-week[data-toggle='tooltip']");
                    if (periodicityElement == null)
                    {
                        Log.Error("Не удалось получить тип недели для пары: {PeriodicityElement}. Пропускаем пару...",
                            periodicityElement);
                        continue;
                    }

                    @class.WeekType = ParseWeekType(periodicityElement.GetAttribute("title")!);

                    var lessonNameElement = row.QuerySelector(".lesson-name");
                    if (lessonNameElement == null)
                    {
                        Log.Error("Не удалось получить название пары: {LessonNameElement}. Пропускаем пару...",
                            lessonNameElement);
                        continue;
                    }

                    var className = lessonNameElement.TextContent.Trim();

                    if (HasWeekNumberCondition().IsMatch(className))
                    {
                        var temp = className.Split("(");
                        className = string.Join("", temp[..^1]).Trim();
                    }

                    @class.Name = className;
                    if (@class.Name.EndsWith(firstSubgroupSuffix))
                    {
                        @class.Name = @class.Name[..^firstSubgroupSuffix.Length];
                        @class.Subgroup = Subgroup.First;
                    }
                    else if (@class.Name.EndsWith(secondSubgroupSuffix))
                    {
                        @class.Name = @class.Name[..^secondSubgroupSuffix.Length];
                        @class.Subgroup = Subgroup.Second;
                    }
                    else
                    {
                        @class.Subgroup = Subgroup.None;
                    }

                    var teacherElement = row.QuerySelector("a[href*='teacher']");
                    if (teacherElement == null)
                    {
                        Log.Error("Не удалось получить имя преподавателя: {TeacherElement}. Пропускаем пару...",
                            teacherElement);
                        continue;
                    }

                    @class.TeacherId = await GetOrCreateTeacherId(
                        teacherElement.TextContent.Replace(". ", ".").Replace(".", ". ").Trim(), cancellationToken);

                    var roomElement = row.QuerySelector("a[href*='room']");
                    if (roomElement == null)
                    {
                        Log.Error("Не удалось получить номер аудитории: {RoomElement}. Пропускаем пару...",
                            roomElement);
                        continue;
                    }

                    @class.LocationId = await GetOrCreateLocationId(roomElement.TextContent.Trim(), cancellationToken);

                    classes.Add(@class);
                }
            }

            var dayModel = new DayParseModel { DayOfWeek = dayOfWeek.Value, Classes = classes };
            days.Add(dayModel);
        }

        return days;
    }

    private static DayOfWeek? ParseDayOfWeek(string dayName)
    {
        return dayName switch
        {
            "ПОНЕДЕЛЬНИК" => DayOfWeek.Monday,
            "ВТОРНИК" => DayOfWeek.Tuesday,
            "СРЕДА" => DayOfWeek.Wednesday,
            "ЧЕТВЕРГ" => DayOfWeek.Thursday,
            "ПЯТНИЦА" => DayOfWeek.Friday,
            "СУББОТА" => DayOfWeek.Saturday,
            "ВОСКРЕСЕНЬЕ" => DayOfWeek.Sunday,
            _ => null
        };
    }

    private static ClassType ParseClassType(string lessonType)
    {
        return lessonType switch
        {
            "пр" => ClassType.Practice,
            "лек" => ClassType.Lecture,
            "лаб" => ClassType.LabWork,
            _ => throw new ArgumentOutOfRangeException(nameof(lessonType), lessonType, null)
        };
    }

    private static WeekType ParseWeekType(string title)
    {
        return title switch
        {
            "Еженедельно" => WeekType.Every,
            "Нечётная неделя" => WeekType.Odd,
            "Чётная неделя" => WeekType.Even,
            _ => throw new ArgumentOutOfRangeException(nameof(title), title, null)
        };
    }

    private async Task<Guid> GetOrCreateTeacherId(string teacherName, CancellationToken cancellationToken = default)
    {
        var existingTeacher =
            await context.Teachers.SingleOrDefaultAsync(x => x.Name == teacherName, cancellationToken);

        if (existingTeacher == null)
        {
            var newTeacher = new Teacher { Name = teacherName };
            await context.Teachers.AddAsync(newTeacher, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return newTeacher.Id;
        }

        return existingTeacher.Id;
    }

    private async Task<Guid> GetOrCreateLocationId(string locationName, CancellationToken cancellationToken = default)
    {
        var existingLocation =
            await context.Locations.SingleOrDefaultAsync(x => x.Name == locationName, cancellationToken);

        if (existingLocation == null)
        {
            var newLocation = new Location { Name = locationName, Type = LocationType.Irl };
            await context.Locations.AddAsync(newLocation, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return newLocation.Id;
        }

        return existingLocation.Id;
    }

    [GeneratedRegex(@".*\(.* недели\)")]
    private static partial Regex HasWeekNumberCondition();
}