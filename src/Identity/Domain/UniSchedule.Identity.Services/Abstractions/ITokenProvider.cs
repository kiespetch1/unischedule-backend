﻿using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Identity.DTO;

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
    /// <param name="returnUrl">Ссылка для возврата после авторизации</param>
    /// <returns>Токен доступа</returns>
    TokenModel IssueToken(TokenIssueContext context, string? returnUrl = null);

    /// <summary>
    ///     Парсинг токена доступа для получения контекста пользователя
    /// </summary>
    /// <param name="accessToken">Строковое значение токена доступа</param>
    /// <returns>Контекст пользователя</returns>
    UserContext ParseToken(string accessToken);
}