namespace UniSchedule.Abstractions.Entities.Auditable;

/// <summary>
///     Интерфейс для создаваемых объектов
/// </summary>
public interface ICreatable
{
    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, создавшего объект
    /// </summary>
    public Guid? CreatedBy { get; set; }
}