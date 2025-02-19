namespace UniSchedule.Abstractions.Helpers.Identity;

/// <summary>
///     Контекст пользователя
/// </summary>
/// <remarks>
///     Класс данных о пользователе, полученных из JWT-токена
/// </remarks>
public class UserContext
{
    /// <summary />
    public UserContext()
    {
        IsAuthenticated = false;
    }

    /// <summary />
    public UserContext(
        Guid userId,
        string surname,
        string name,
        string patronymic,
        string email,
        List<Guid> managedGroupIds,
        Guid? groupId,
        string role) : this()
    {
        IsAuthenticated = true;
        Id = userId;
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        Email = email;
        ManagedGroupIds = managedGroupIds;
        GroupId = groupId;
        Role = role;
    }

    /// <summary>
    ///     Авторизован ли пользователь
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

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
    public string? Email { get; set; }

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