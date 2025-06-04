namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры для отмены всех пар группы
/// </summary>
public class ClassCancelByGroupIdParameters
{
    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid GroupId { get; set; }
}
