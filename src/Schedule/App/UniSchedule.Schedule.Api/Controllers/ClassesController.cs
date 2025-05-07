using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Abstractions.Commands;
using UniSchedule.Abstractions.Queries.Base;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Data;
using UniSchedule.Identity.Shared;
using UniSchedule.Identity.Shared.Attributes;
using UniSchedule.Schedule.Entities;
using UniSchedule.Schedule.Services.Abstractions;
using UniSchedule.Shared.DTO.Models;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Api.Controllers;

/// <summary>
///     Контроллер для работы с парами
/// </summary>
[ApiController]
[Route("/api/v1/[controller]")]
public class ClassesController(
    ICreateCommand<Class, ClassCreateParameters, Guid> create,
    IDeleteCommand<Class, Guid> delete,
    IUpdateCommand<Class, ClassUpdateParameters, Guid> update,
    IMultipleQuery<Class, ClassQueryParameters> get,
    ISingleQuery<Class, Guid> getById,
    IClassService service,
    IMapper mapper)
{
    /// <summary>
    ///     Создание пары
    /// </summary>
    /// <param name="parameters">Параметры создания пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор пары</returns>
    /// <response code="200">Успешное создание пары</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task<Result<Guid>> CreateAsync(
        [FromBody] ClassCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var id = await create.ExecuteAsync(parameters, cancellationToken);

        return new Result<Guid>(id);
    }

    /// <summary>
    ///     Получение пары по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Пара</returns>
    /// <response code="200">Успешное получение пары</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<ClassModel>> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var @class = await getById.ExecuteAsync(id, cancellationToken);
        var result = mapper.Map<ClassModel>(@class);

        return new Result<ClassModel>(result);
    }

    /// <summary>
    ///     Получение списка пар
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список пар</returns>
    /// <response code="200">Успешное получение пар</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<CollectionResult<ClassModel>> GetAsync(
        [FromQuery] ClassQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var classes = await get.ExecuteAsync(parameters, cancellationToken);
        var data = mapper.Map<CollectionResult<ClassModel>>(classes);

        return data;
    }

    /// <summary>
    ///     Обновление пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="parameters">Параметры обновления пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное обновление пары</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPut("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] ClassUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await update.ExecuteAsync(id, parameters, cancellationToken);
    }

    /// <summary>
    ///     Удаление пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное удаление пары</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpDelete("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await delete.ExecuteAsync(id, cancellationToken);
    }
    /// <summary>
    ///     Отмена пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешная отмена пары</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("{id}/cancel")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task CancelAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await service.SetCancelledAsync(id, cancellationToken);
    }

    /// <summary>
    ///     Восстановление пары
    /// </summary>
    /// <param name="id">Идентификатор пары</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешное восстановление пары</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("{id}/restore")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task RestoreAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await service.SetActiveAsync(id, cancellationToken);
    }

    /// <summary>
    ///     Копирование пар дня на противоположную неделю
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешное копирование пар</response>
    /// <response code="404">День/противоположная неделя не найдены</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("copy/{dayId}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task CopyClassesToOppositeWeekAsync(
        [FromRoute] Guid dayId,
        CancellationToken cancellationToken = default)
    {
        await service.CopyClassesToOppositeWeekAsync(dayId, cancellationToken);
    }

    /// <summary>
    ///     Удаление всех пар дня
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешное удаление всех пар дня</response>
    /// <response code="404">День не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost("clear/{dayId}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task ClearDayClassesAsync(
        [FromRoute] Guid dayId,
        CancellationToken cancellationToken = default)
    {
        await service.ClearDayClassesAsync(dayId, cancellationToken);
    }
}