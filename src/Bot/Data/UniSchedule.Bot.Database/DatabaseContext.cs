using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers.Database;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Bot.Entities.Auxiliary;
using UniSchedule.Entities;
using UniSchedule.Events.Shared.Parameters;

namespace UniSchedule.Bot.Database;

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
    ///     Группы
    /// </summary>
    public DbSet<Group> Groups { get; set; }
    
    /// <summary>
    ///     Связь пользователя между мессенджером и системой
    /// </summary>
    public DbSet<UserMessengerUser> UserMessengerUser { get; set; }

    /// <summary>
    ///     Связь группы в системе с беседой группы в мессенджере 
    /// </summary>
    public DbSet<GroupMessengerConversation> GroupMessengerConversation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}