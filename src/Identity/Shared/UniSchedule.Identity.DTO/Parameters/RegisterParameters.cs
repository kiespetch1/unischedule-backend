namespace UniSchedule.Identity.DTO.Parameters;

/// <summary>
///     Параметры для регистрации пользователя
/// </summary>
public class RegisterParameters
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
    public string? Patronymic { get; set; }

    /// <summary>
    ///     Электронная почта
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    ///     Пароль пользователя
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    ///     Идентификатор роли
    /// </summary>
    public required Guid RoleId { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid? GroupId { get; set; }

    /// <summary>
    ///     Список идентификаторов групп, которыми управляет пользователь
    /// </summary>
    public List<Guid>? ManagedGroupIds { get; set; }
}