namespace UniSchedule.Identity.DTO.Messages;

/// <summary>
///     Модель пользователя для передачи через брокер сообщений
/// </summary>
public class UserMqModel
{
    /// <summary>
    ///     Идентификатор
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
    ///     Электронная почта
    /// </summary>
    public string Email { get; set; }
}