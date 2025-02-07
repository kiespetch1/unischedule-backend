namespace UniSchedule.Abstractions.Entities.Auditable;

/// <summary>
///     Интерфейс для soft-удаляемых объектов
/// </summary>
public interface IDeletable
{
    /// <summary>
    ///     Дата удаления
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, удалившего объект
    /// </summary>
    public Guid? DeletedBy { get; set; }
}