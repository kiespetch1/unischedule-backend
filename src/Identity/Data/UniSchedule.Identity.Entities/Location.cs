using UniSchedule.Identity.Entities.Enums;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Место проведения занятий
/// </summary>
public class Location
{
    /// <summary>
    ///     Название
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Ссылка на встречу
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    ///     Тип
    /// </summary>
    public LocationType LocationType { get; set; }
}