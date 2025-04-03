using UniSchedule.Abstractions.Entities;

namespace UniSchedule.Entities;

public class User : Entity<Guid>
{
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

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Surname} {Name} {Patronymic}";
    }
}