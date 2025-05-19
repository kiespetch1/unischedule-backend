namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания группы
/// </summary>
public class GroupCreateParameters
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Курс
    /// </summary>
    public required int Grade { get; set; }

    /// <summary>
    ///     Имеет ли разделение по подгруппам
    /// </summary>
    public required bool HasSubgroups { get; set; }

    /// <summary>
    ///     Имеет ли четкое разделение на подгруппы
    /// </summary>
    public required bool HasFixedSubgroups { get; set; }

    /// <summary>
    ///     Номер последней учебной недели (без учета зачетных и экзаменационных)
    /// </summary>
    public int LastAcademicWeekNumber { get; set; } = 16;
}