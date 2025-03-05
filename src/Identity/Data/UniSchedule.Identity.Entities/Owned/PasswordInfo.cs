namespace UniSchedule.Identity.Entities.Owned;

/// <summary>
///     Информация о пароле
/// </summary>
public class PasswordInfo
{
    /// <summary>
    ///     Хэш пароля
    /// </summary>
    public string Hash { get; set; }

    /// <summary>
    ///     Соль
    /// </summary>
    public string Salt { get; set; }
}