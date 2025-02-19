using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers.Database;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Events.Entities;
using UniSchedule.Events.Shared.Parameters;
using Action = UniSchedule.Events.Entities.Action;

namespace UniSchedule.Events.Database;

/// <summary>
///     Контекст базы данных
/// </summary>
public class DatabaseContext : AuditableDbContext
{
    public DatabaseContext(
        DbContextOptions<DatabaseContext> options,
        IPublisher<EventCreateParameters>? publisher = null,
        IUserContextProvider? userProvider = null)
        : base(options, publisher, userProvider)
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///     События
    /// </summary>
    public DbSet<Event> Events { get; set; }

    /// <summary>
    ///     Предмет изменения
    /// </summary>
    public DbSet<Subject> Subjects { get; set; }

    /// <summary>
    ///     Действия пользователя
    /// </summary>
    public DbSet<Action> Actions { get; set; }

    /// <summary>
    ///     Записи изменений
    /// </summary>
    public DbSet<Change> Changes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}