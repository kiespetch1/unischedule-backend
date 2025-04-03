using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.DTO.Models;

/// <summary>
///     Модель контекста пользователя
/// </summary>
public class UserContextModel
{
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
    public string Patronymic { get; set; }

    /// <summary>
    ///     Email пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Список ролей пользователя
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    ///     Список идентификаторов управляемых групп
    /// </summary>
    public List<Guid> ManagedGroupIds { get; set; }
}