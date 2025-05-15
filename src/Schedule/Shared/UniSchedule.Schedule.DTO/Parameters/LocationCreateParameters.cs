using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания места проведения
/// </summary>
public class LocationCreateParameters
{
    /// <summary>
    ///     Название
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Ссылка на встречу
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    ///     Тип
    /// </summary>
    public required LocationType Type { get; set; }
}