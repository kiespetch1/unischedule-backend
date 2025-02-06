namespace UniSchedule.Abstractions.Entities.Auditable;

/// <summary>
///     Интерфейс для архивируемых сущностей
/// </summary>
public interface IArchivable
{
    /// <summary>
    ///     Дата архивации
    /// </summary>
    public DateTime? ArchivedAt { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, который архивировал запись
    /// </summary>
    public Guid? ArchivedBy { get; set; }

    /// <summary>
    ///     Архивировано значение справочника
    /// </summary>
    /// <returns>True - архивировано, False - не архивировано</returns>
    bool IsArchived()
    {
        return ArchivedAt.HasValue;
    }
}