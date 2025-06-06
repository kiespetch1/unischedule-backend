namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры для импорта расписания занятий группы
/// </summary>
public class ClassScheduleImportParameters
{
    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    ///     URL-адрес для импорта расписания
    /// </summary>
    public string Url { get; set; } = string.Empty;
}