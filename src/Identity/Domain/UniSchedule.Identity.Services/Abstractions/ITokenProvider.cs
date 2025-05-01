using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Identity.Entities;

namespace UniSchedule.Identity.Services.Abstractions;

/// <summary>
///     Провайдер для управления токенами доступа
/// </summary>
public interface ITokenProvider
{
    /// <summary>
    ///     Выдача токена доступа
    /// </summary>
    /// <param name="context">Контекст выдачи токена доступа</param>
    /// <returns>Токен доступа</returns>
    Token IssueToken(TokenIssueContext context);

    /// <summary>
    ///     Парсинг токена доступа для получения контекста пользователя
    /// </summary>
    /// <param name="accessToken">Строковое значение токена доступа</param>
    /// <returns>Контекст пользователя</returns>
    UserContext ParseToken(string accessToken);
}