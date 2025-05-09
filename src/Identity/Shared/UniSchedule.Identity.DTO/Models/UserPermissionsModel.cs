namespace UniSchedule.Identity.DTO.Models;

/// <summary>
///     Контекст разрешений пользователя
/// </summary>
public class UserPermissionsModel
{
    /// <summary>
    ///     Может ли регистрировать новых пользователей
    /// </summary>
    public bool CanRegisterUser { get; set; }

    /// <summary>
    ///     Может ли обновлять информацию о пользователях
    /// </summary>
    public bool CanUpdateUser { get; set; }

    /// <summary>
    ///     Может ли получать данные о текущем аутентифицированном пользователе
    /// </summary>
    public bool CanGetCurrentUser { get; set; }

    /// <summary>
    ///     Может ли создавать объявления
    /// </summary>
    public bool CanCreateAnnouncement { get; set; }

    /// <summary>
    ///     Может ли обновлять объявления
    /// </summary>
    public bool CanUpdateAnnouncement { get; set; }

    /// <summary>
    ///     Может ли удалять объявления
    /// </summary>
    public bool CanDeleteAnnouncement { get; set; }

    /// <summary>
    ///     Может ли создавать пары
    /// </summary>
    public bool CanCreateClass { get; set; }

    /// <summary>
    ///     Может ли обновлять пары
    /// </summary>
    public bool CanUpdateClass { get; set; }

    /// <summary>
    ///     Может ли удалять пары
    /// </summary>
    public bool CanDeleteClass { get; set; }

    /// <summary>
    ///     Может ли отменять пары
    /// </summary>
    public bool CanCancelClass { get; set; }

    /// <summary>
    ///     Может ли восстанавливать пары
    /// </summary>
    public bool CanRestoreClass { get; set; }

    /// <summary>
    ///     Может ли копировать пары на противоположную неделю
    /// </summary>
    public bool CanCopyClass { get; set; }


    /// <summary>
    ///     Может ли создавать группы
    /// </summary>
    public bool CanCreateGroup { get; set; }

    /// <summary>
    ///     Может ли обновлять группы
    /// </summary>
    public bool CanUpdateGroup { get; set; }

    /// <summary>
    ///     Может ли удалять группы
    /// </summary>
    public bool CanDeleteGroup { get; set; }

    /// <summary>
    ///     Может ли обновлять курс групп
    /// </summary>
    public bool CanUpdateGrades { get; set; }

    /// <summary>
    ///     Может ли создавать места проведения
    /// </summary>
    public bool CanCreateLocation { get; set; }

    /// <summary>
    ///     Может ли обновлять места проведения
    /// </summary>
    public bool CanUpdateLocation { get; set; }

    /// <summary>
    ///     Может ли удалять места проведения
    /// </summary>
    public bool CanDeleteLocation { get; set; }

    /// <summary>
    ///     Может ли создавать преподавателей
    /// </summary>
    public bool CanCreateTeacher { get; set; }

    /// <summary>
    ///     Может ли обновлять преподавателей
    /// </summary>
    public bool CanUpdateTeacher { get; set; }

    /// <summary>
    ///     Может ли удалять преподавателей
    /// </summary>
    public bool CanDeleteTeacher { get; set; }

    /// <summary>
    ///     Может ли создавать недели
    /// </summary>
    public bool CanCreateWeek { get; set; }

    /// <summary>
    ///     Может ли удалять недели
    /// </summary>
    public bool CanDeleteWeek { get; set; }
}