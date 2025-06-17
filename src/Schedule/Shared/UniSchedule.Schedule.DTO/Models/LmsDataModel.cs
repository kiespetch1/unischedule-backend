namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель данных LMS
/// </summary>
public class LmsDataModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Название предмета
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    ///     Данные доступа
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }
}