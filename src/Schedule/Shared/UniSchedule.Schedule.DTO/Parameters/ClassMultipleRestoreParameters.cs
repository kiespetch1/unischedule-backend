namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры для восстановления нескольких пар
/// </summary>
public class ClassMultipleRestoreParameters
{
    /// <summary>
    ///     Список идентификаторов пар для восстановления
    /// </summary>
    public required List<Guid> ClassIds { get; set; }
}