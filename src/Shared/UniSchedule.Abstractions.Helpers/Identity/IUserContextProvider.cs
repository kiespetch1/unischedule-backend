namespace UniSchedule.Abstractions.Helpers.Identity;

public interface IUserContextProvider
{
    /// <summary>
    ///     Проверяет, аутентифицирован ли пользователь
    /// </summary>
    public bool IsAuthenticated();

    /// <summary>
    ///     Получает контекст пользователя
    /// </summary>
    /// <returns>Контекст пользователя <see cref="UserContext" /></returns>
    public UserContext GetContext();

    /// <summary>
    ///     Получает разрешения пользователя
    /// </summary>
    /// <returns></returns>
    public UserPermissions GetPermissions(); 
}