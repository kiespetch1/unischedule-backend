namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры создания пары
/// </summary>
public class ClassCreateParameters : ClassUpdateParameters
{
    /// <summary>
    ///     Идентификатор дня
    /// </summary>
    public required Guid DayId { get; set; }
}