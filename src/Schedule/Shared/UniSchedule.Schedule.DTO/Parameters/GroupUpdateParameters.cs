namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры обновления группы
/// </summary>
public class GroupUpdateParameters
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
}