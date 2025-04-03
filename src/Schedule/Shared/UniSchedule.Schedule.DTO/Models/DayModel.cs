namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель дня
/// </summary>
public class DayModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     День недели
    /// </summary>
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    ///     Идентификатор недели
    /// </summary>
    public Guid WeekId { get; set; }

    /// <summary>
    ///     Пары
    /// </summary>
    public ICollection<ClassModel> Classes { get; set; }
}