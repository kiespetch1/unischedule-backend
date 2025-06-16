using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Abstractions.Helpers.Identity;
using UniSchedule.Abstractions.Queries.Base;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Data;
using UniSchedule.Identity.Shared.Attributes;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Services.Abstractions;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Api.Controllers;

/// <summary>
///     API для работы с авторизацией
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AccountController(
    IPreferencesService preferencesService,
    IUserContextProvider userContextProvider,
    IMultipleQuery<ScheduleFilteringOption, ScheduleFilteringQueryParameters> getFiltering,
    ISingleQuery<ScheduleFilteringOption, Guid> getFilteringById,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Обновление antiforgery токена
    /// </summary>
    /// <response code="200">Успешное обновление токена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("antiforgery")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public void RefreshAntiforgery() { }

    /// <summary>
    ///     Получение предпочтений фильтрации расписания по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор предпочтения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Предпочтение фильтрации</returns>
    /// <response code="200">Успешное получение предпочтения</response>
    /// <response code="404">Предпочтение не найдено</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("preferences/filtering/{id:guid}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<ScheduleFilteringModel> GetFilteringByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await getFilteringById.ExecuteAsync(id, cancellationToken);
        var result = mapper.Map<ScheduleFilteringModel>(entity);

        return result;
    }

    /// <summary>
    ///     Получение списка предпочтений фильтрации расписания
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список предпочтений фильтрации</returns>
    /// <response code="200">Успешное получение предпочтений</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("preferences/filtering")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    [Authorize]
    public async Task<CollectionResult<ScheduleFilteringOption>> GetFilteringAsync(
        [FromQuery] ScheduleFilteringQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var filtering = await getFiltering.ExecuteAsync(parameters, cancellationToken);

        return filtering;
    }

    /// <summary>
    ///     Установка предпочтений фильтрации расписания
    /// </summary>
    /// <param name="parameters">Параметры фильтрации</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешное сохранение предпочтений</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("preferences/filtering")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    [Authorize]
    public async Task SetFilterPreferencesAsync(
        [FromBody] ScheduleFilteringParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var userId = userContextProvider.GetContext().Id;
        await preferencesService.SetMultipleAsync(parameters, userId, cancellationToken);
    }

    /// <summary>
    ///     Удаление предпочтения фильтрации расписания
    /// </summary>
    /// <param name="id">Идентификатор предпочтения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешное удаление предпочтения</response>
    /// <response code="404">Предпочтение не найдено</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpDelete("preferences/filtering/{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize]
    public async Task DeletePreferenceAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var userId = userContextProvider.GetContext().Id;
        await preferencesService.DeleteAsync(id, userId, cancellationToken);
    }
}