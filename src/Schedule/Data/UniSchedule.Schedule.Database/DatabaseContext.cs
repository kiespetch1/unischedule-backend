using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers.Database;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Entities;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Schedule.Entities;
using Group = UniSchedule.Schedule.Entities.Group;

namespace UniSchedule.Schedule.Database;

/// <summary>
///     Контекст базы данных
/// </summary>
public class DatabaseContext(
    DbContextOptions<DatabaseContext> options,
    IPublisher<EventCreateParameters>? publisher = null,
    IUserContextProvider? userProvider = null)
    : AuditableDbContext(options, publisher, userProvider)
{
    /// <summary>
    ///     Преподаватели
    /// </summary>
    public DbSet<Teacher> Teachers { get; set; }

    /// <summary>
    ///     Пары
    /// </summary>
    public DbSet<Class> Classes { get; set; }

    /// <summary>
    ///     Дни
    /// </summary>
    public DbSet<Day> Days { get; set; }

    /// <summary>
    ///     Недели
    /// </summary>
    public DbSet<Week> Weeks { get; set; }

    /// <summary>
    ///     Группы
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    ///     Объявления
    /// </summary>
    public DbSet<Announcement> Announcements { get; set; }

    /// <summary>
    ///     Места проведения занятий
    /// </summary>
    public DbSet<Location> Locations { get; set; }

    /// <summary>
    ///     Пользователи
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///     Данные для доступа к LMS
    /// </summary>
    public DbSet<LmsData> LmsData { get; set; }

    /// <summary>
    ///     Информация о фильтрации
    /// </summary>
    public DbSet<ScheduleFilteringOption> FilteringInfo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}