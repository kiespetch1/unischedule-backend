namespace UniSchedule.Schedule.Entities.Owned;

/// <summary>
///     Данные блока объявления
/// </summary>
public class AnnouncementsBlock
{
    /// <summary>
    ///     Последнее объявление
    /// </summary>
    public Announcement? Last { get; set; }

    /// <summary>
    ///     Последнее объявление с временным ограничением
    /// </summary>
    public Announcement? LastTimeLimited { get; set; }
}