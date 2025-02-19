using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Events.Entities;
using UniSchedule.Events.Shared.Parameters;
using UniSchedule.Extensions.Basic;
using UniSchedule.Identity.Shared;

namespace UniSchedule.Abstractions.Helpers.Database;

public class AuditHelper(IUserContextProvider? userProvider)
{
    /// <summary>
    ///     Создаёт объект аудита <see cref="Event" /> на основе отслеживаемой записи
    /// </summary>
    public EventCreateParameters CreateAuditEvent(EntityEntry entry)
    {
        var auditEvent = new EventCreateParameters
        {
            ActionId = MapActionOption(entry.State),
            SubjectId = GetSubjectId(entry.Entity),
            FieldChanges = GetFieldChanges(entry),
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = userProvider?.GetContext().Id ?? Guid.Empty
        };

        return auditEvent;
    }

    /// <summary>
    ///     Преобразует состояние сущности в соответствующий тип действия
    /// </summary>
    private static Guid MapActionOption(EntityState state)
    {
        return state switch
        {
            EntityState.Added => ActionOption.Create.GetId(),
            EntityState.Modified => ActionOption.Update.GetId(),
            EntityState.Deleted => ActionOption.Delete.GetId(),
            _ => throw new NotSupportedException("Отслеживание этого статуса не поддерживается")
        };
    }

    /// <summary>
    ///     Определяет субъект изменения на основе типа сущности
    /// </summary>
    private static Guid GetSubjectId(object entity)
    {
        var entityName = entity.GetType().Name.ToLower();

        var id = entityName switch
        {
            _ when entityName.Contains("user") => SubjectOption.Users.GetId(),
            _ when entityName.Contains("announcement") => SubjectOption.Announcements.GetId(),
            _ when entityName.Contains("group") => SubjectOption.Groups.GetId(),
            _ when entityName.Contains("class") => SubjectOption.Class.GetId(),
            _ when entityName.Contains("day") => SubjectOption.Day.GetId(),
            _ when entityName.Contains("week") => SubjectOption.Week.GetId(),
            _ => throw new NotSupportedException("Отслеживание этой сущности не поддерживается")
        };

        return id;
    }

    /// <summary>
    ///     Формирует список изменений для записи аудита
    /// </summary>
    private static List<ChangeCreateParameters> GetFieldChanges(EntityEntry entry)
    {
        var changes = new List<ChangeCreateParameters>();

        switch (entry.State)
        {
            case EntityState.Added:
            {
                changes.AddRange(entry.Properties.Select(property => new ChangeCreateParameters
                {
                    FieldName = property.Metadata.Name,
                    OldValue = null,
                    NewValue = property.CurrentValue?.ToString() ?? string.Empty
                }));

                break;
            }
            case EntityState.Deleted:
            {
                changes.AddRange(entry.Properties.Select(property => new ChangeCreateParameters
                {
                    FieldName = property.Metadata.Name,
                    OldValue = property.OriginalValue?.ToString() ?? string.Empty,
                    NewValue = string.Empty,
                    IsDeleted = true
                }));

                break;
            }
            case EntityState.Modified:
            {
                changes.AddRange(from property in entry.Properties
                    where property.IsModified
                    select new ChangeCreateParameters
                    {
                        FieldName = property.Metadata.Name,
                        OldValue = property.OriginalValue?.ToString() ?? string.Empty,
                        NewValue = property.CurrentValue?.ToString() ?? string.Empty
                    });

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(entry), "Отслеживание этого статуса не поддерживается");
        }

        return changes;
    }
}