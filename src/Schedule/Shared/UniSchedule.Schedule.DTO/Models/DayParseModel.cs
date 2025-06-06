namespace UniSchedule.Shared.DTO.Models;

public class DayParseModel
{
    /// <summary>
    ///     День недели
    /// </summary>
    public required DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    ///     Идентификатор недели
    /// </summary>
    public Guid WeekId { get; set; }

    /// <summary>
    ///     Пары
    /// </summary>
    public ICollection<ClassParseModel> Classes { get; set; }
}