using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Helpers;

namespace UniSchedule.Abstractions.Helpers.Database;

public class AuditableDbContext : DbContext, IMigrationDatabase
{
    /// <inheritdoc cref="IUserContextProvider" />
    private readonly IUserContextProvider? _userProvider;

    /// <inheritdoc cref="AuditHelper" />
    private readonly AuditHelper _auditHelper;

    /// <inheritdoc cref="IPublisher{T}" />
    private readonly IPublisher<EventCreateParameters>? _publisher;

    protected AuditableDbContext(
        DbContextOptions options,
        IPublisher<EventCreateParameters>? publisher = null,
        IUserContextProvider? userProvider = null) :
        base(options)
    {
        _userProvider = userProvider;
        _auditHelper = new AuditHelper(_userProvider);
    }

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        WriteAuditedEntitiesChanges().GetAwaiter().GetResult();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        await WriteAuditedEntitiesChanges();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    ///     Обходит все изменённые сущности с атрибутом <see cref="AuditableAttribute" />
    ///     и публикует для них события аудита через IAuditEventPublisher.
    /// </summary>
    private async Task WriteAuditedEntitiesChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().GetCustomAttribute<AuditableAttribute>() != null &&
                        e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var auditEvent in entries.Select(entry => _auditHelper.CreateAuditEvent(entry)))
        {
            await _publisher.PublishAsync(auditEvent);
        }
    }
}