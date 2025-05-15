using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UniSchedule.Abstractions.Entities.Auditable;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Messaging;
using UniSchedule.Events.Shared.Parameters;

namespace UniSchedule.Abstractions.Helpers.Database;

/// <summary>
///     Перехватчик запросов на сохранение изменений в БД
/// </summary>
public class AuditableInterceptor(IUserContextProvider? userProvider, IPublisher<EventCreateParameters>? publisher = null)
    : SaveChangesInterceptor
{
    private Guid ActorId => userProvider?.GetContext().Id ?? Guid.Empty;  
    
    /// <inheritdoc />
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            SetCreatableInfo(eventData.Context);
            SetUpdatableInfo(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            SetCreatableInfo(eventData.Context);
            SetUpdatableInfo(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    ///     Установка значений о создании
    /// </summary>
    private void SetCreatableInfo(DbContext context)
    {
        var createdEntries = context.ChangeTracker
            .Entries<ICreatable>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in createdEntries)
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.CreatedBy = ActorId;
        }
    }

    /// <summary>
    ///     Установка значений об обновлении
    /// </summary>
    private void SetUpdatableInfo(DbContext context)
    {
        var updatedEntries = context.ChangeTracker
            .Entries<IUpdatable>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        foreach (var entry in updatedEntries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedBy = ActorId;
        }
    }
}