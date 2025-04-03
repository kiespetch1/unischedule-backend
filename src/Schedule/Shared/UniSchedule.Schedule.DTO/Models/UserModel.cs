namespace UniSchedule.Shared.DTO.Models;

public class UserModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Фамилия
    /// </summary>
    public required string Surname { get; set; }

    /// <summary>
    ///     Имя
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Отчество
    /// </summary>
    public required string? Patronymic { get; set; }

    /// <summary>
    ///     Электронная почта
    /// </summary>
    public required string Email { get; set; }
}