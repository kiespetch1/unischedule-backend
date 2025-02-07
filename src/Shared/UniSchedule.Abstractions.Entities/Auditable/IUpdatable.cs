namespace UniSchedule.Abstractions.Entities.Auditable;

/// <summary>
///     Интерфейс для обновляемых объектов
/// </summary>
public interface IUpdatable
{
    /// <summary>
    ///     Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, обновившего объект
    /// </summary>
    public Guid? UpdatedBy { get; set; }
}