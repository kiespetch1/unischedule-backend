namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель данных блока объявлений
/// </summary>
public class AnnouncementsBlockModel
{
    /// <summary>
    ///     Последнее объявление
    /// </summary>
    public AnnouncementModel? Last { get; set; }

    /// <summary>
    ///     Последнее объявление с временным ограничением
    /// </summary>
    public AnnouncementModel? LastTimeLimited { get; set; }
}