namespace UniSchedule.Shared.DTO.Parameters;

/// <summary>
///     Параметры обновления пары
/// </summary>
public class ClassUpdateParameters : ClassCreateParameters
{
    /// <summary>
    ///     Идентификатор пары
    /// </summary>
    public required Guid Id { get; set; }
}