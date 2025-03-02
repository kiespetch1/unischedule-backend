using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.Services.Abstractions;

/// <summary>
///     Провайдер для работы с контекстом создания токена доступа
/// </summary>
public interface ITokenContextProvider
{
    /// <summary>
    ///     Формирование контекста для создания токена доступа
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <returns>Контекст для создания токена доступа <see cref="TokenIssueContext" /></returns>
    TokenIssueContext CreateContext(User user);
}