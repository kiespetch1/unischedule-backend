namespace UniSchedule.Shared.DTO.Models;

/// <summary>
///     Модель преподавателя
/// </summary>
public class TeacherModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     ФИО преподавателя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Полное ФИО преподавателя
    /// </summary>
    public string FullName { get; set; }
}