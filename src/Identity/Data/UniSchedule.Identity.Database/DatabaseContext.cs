using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers.Database;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.Database;

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
    ///     Роли
    /// </summary>
    public DbSet<Role> Role { get; set; }

    /// <summary>
    ///     Пользователи
    /// </summary>
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}