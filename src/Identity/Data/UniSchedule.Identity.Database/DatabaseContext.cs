using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.Database;

/// <summary>
///     Контекст базы данных
/// </summary>
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
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
    ///     Уведомления
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

    /// <summary>
    ///     Места проведения занятий
    /// </summary>
    public DbSet<Location> Locations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}