namespace UniSchedule.Abstractions.Helpers.Identity;

/// <summary>
///     Данные, сохраняемые в токене
/// </summary>
public class TokenIssueContext
{
    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     Фамилия
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    ///     Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Отчество
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    ///     Email пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Роль пользователя
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid? GroupId { get; set; }

    /// <summary>
    ///     Список идентификаторов групп, которыми управляет пользователь
    /// </summary>
    public List<Guid>? ManagedGroupIds { get; set; }
}