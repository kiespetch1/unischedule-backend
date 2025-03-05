using UniSchedule.Abstractions.Entities;
using UniSchedule.Identity.Entities.Owned;

namespace UniSchedule.Identity.Entities;

public class User : Entity<Guid>
{
    /// <summary>
    ///     Пароль
    /// </summary>
    public required PasswordInfo Password { get; set; }

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

    /// <summary>
    ///     Идентификатор роли
    /// </summary>
    public required Guid RoleId { get; set; }

    /// <summary>
    ///     Роль
    /// </summary>
    public required Role Role { get; set; }

    /// <summary>
    ///     Идентификатор группы
    /// </summary>
    public Guid? GroupId { get; set; }

    /// <summary>
    ///     Список идентификаторов групп, которыми управляет пользователь
    /// </summary>
    public List<Guid>? ManagedGroupIds { get; set; }

    /// <summary>
    ///     Токен обновления
    /// </summary>
    public string? RefreshToken { get; set; }
}