namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания преподавателя
/// </summary>
public class TeacherCreateParameters
{
    /// <summary>
    ///     ФИО преподавателя
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Полное ФИО преподавателя
    /// </summary>
    public string? FullName { get; set; }
}