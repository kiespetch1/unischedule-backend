using UniSchedule.Schedule.Entities.Enums;

namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель места проведения
/// </summary>
public class LocationModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Название
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Ссылка на встречу
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    ///     Тип
    /// </summary>
    public LocationType LocationType { get; set; }
}