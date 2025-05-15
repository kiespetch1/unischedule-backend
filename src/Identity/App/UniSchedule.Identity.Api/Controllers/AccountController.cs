using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Data;
using UniSchedule.Extensions.Exceptions;
using UniSchedule.Identity.DTO.Models;
using UniSchedule.Identity.DTO.Parameters;
using UniSchedule.Identity.Entities;
using UniSchedule.Identity.Services.Abstractions;
using UniSchedule.Identity.Shared;
using UniSchedule.Identity.Shared.Attributes;

namespace UniSchedule.Identity.Api.Controllers;

/// <summary>
///     API для работы с авторизацией
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AccountController(
    IAuthService authenticationService,
    IUserContextProvider userContextProvider,
    ICreateCommand<User, RegisterParameters, Guid> create,
    IUpdateCommand<User, UserUpdateParameters, Guid> update,
    IAntiforgery antiforgery,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Авторизация
    /// </summary>
    /// <param name="parameters">Параметры авторизации</param>
    /// <returns>Модель токена</returns>
    /// <response code="400">Некорректные данные</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("sign_in")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task SignInAsync(SignInParameters parameters)
    {
        var token = await authenticationService.SignInAsync(parameters);

        HttpContext.Response.Cookies.Append("x-token", token.AccessToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromMinutes(30), Domain = ".streaminginfo.ru" });
        HttpContext.Response.Cookies.Append("z-token", token.RefreshToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromDays(30), Domain = ".streaminginfo.ru" });
        antiforgery.GetAndStoreTokens(HttpContext);
    }

    /// <summary>
    ///     Регистрация пользователя
    /// </summary>
    /// <param name="parameters">Параметры регистрации</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданного пользователя</returns>
    /// <response code="200">Успешная регистрация</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="404">Роль не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("register")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin)]
    public async Task<Result<Guid>> RegisterAsync(
        RegisterParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var userId = await create.ExecuteAsync(parameters, cancellationToken);

        return new Result<Guid>(userId);
    }

    /// <summary>
    ///     Выход из системы
    /// </summary>
    /// <response code="200">Успешный выход из системы</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("sign_out")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public new void SignOut()
    {
        Response.Cookies.Delete("x-token", new CookieOptions { Domain = ".streaminginfo.ru" });
        Response.Cookies.Delete("z-token", new CookieOptions { Domain = ".streaminginfo.ru" });
        Response.Cookies.Delete("XSRF-COOKIE", new CookieOptions { Domain = ".streaminginfo.ru" });
    }

    /// <summary>
    ///     Обновление данных пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="parameters">Параметры обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное обновление данных</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">Доступ запрещен</response>
    /// <response code="404">Пользователь или роль не найдены</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPut("update/{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin)]
    public async Task UpdateUserAsync(
        Guid id,
        UserUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await update.ExecuteAsync(id, parameters, cancellationToken);
    }

    /// <summary>
    ///     Обновление токена
    /// </summary>
    /// <returns>Модель токена</returns>
    /// <response code="200">Успешное обновление токена</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">Доступ запрещен</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("refresh")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task RefreshAsync()
    {
        var refreshToken = HttpContext.Request.Cookies["z-token"];
        var expiredToken = HttpContext.Request.Cookies["x-token"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new NotAuthorizedException("Refresh token not found");
        }

        if (string.IsNullOrEmpty(expiredToken))
        {
            throw new NotAuthorizedException("Access token not found");
        }

        var parameters = new RefreshParameters { ExpiredToken = expiredToken, RefreshToken = refreshToken };
        var token = await authenticationService.RefreshAsync(parameters);

        HttpContext.Response.Cookies.Append("x-token", token.AccessToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromMinutes(30), Domain = ".streaminginfo.ru" });
        HttpContext.Response.Cookies.Append("z-token", token.RefreshToken,
            new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromDays(30), Domain = ".streaminginfo.ru" });
    }

    /// <summary>
    ///     Обновление antiforgery токена
    /// </summary>
    /// <response code="200">Успешное обновление токена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("antiforgery")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public void RefreshAntiforgery()
    {
    }

    /// <summary>
    ///     Получение информации о текущем пользователе
    /// </summary>
    /// <returns>Сокращенная модель "Пользователь"</returns>
    /// <response code="200">Успешное получение информации</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("me")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize]
    public Result<UserContextModel> GetCurrent()
    {
        var currentUserContext = userContextProvider.GetContext();
        var user = mapper.Map<UserContextModel>(currentUserContext);

        return new Result<UserContextModel>(user);
    }

    /// <summary>
    ///     Получение разрешений о текущем пользователе
    /// </summary>
    /// <returns>Модель разрешений пользователя</returns>
    /// <response code="200">Успешное получение информации</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("me/permissions")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize]
    public Result<UserPermissions> GetPermissions()
    {
        var currentUserPermissions = userContextProvider.GetPermissions();
        var user = mapper.Map<UserPermissions>(currentUserPermissions);

        return new Result<UserPermissions>(user);
    }
}