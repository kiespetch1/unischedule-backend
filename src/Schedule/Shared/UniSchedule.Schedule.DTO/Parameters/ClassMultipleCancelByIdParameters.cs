namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры отмены нескольких пар по идентификаторам пар
/// </summary>
public class ClassMultipleCancelByIdParameters
{
    /// <summary>
    ///     Список идентификаторов пар
    /// </summary>
    public List<Guid> ClassIds { get; set; }
}