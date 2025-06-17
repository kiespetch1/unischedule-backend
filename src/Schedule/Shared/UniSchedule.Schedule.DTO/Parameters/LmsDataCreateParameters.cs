namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания данных LMS
/// </summary>
public class LmsDataCreateParameters
{
    /// <summary>
    ///     Название предмета
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    ///     Данные доступа
    /// </summary>
    public required string Data { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }
}