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
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
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
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPut("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
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
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpDelete("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
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
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("cancel/{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
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
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пара не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("restore/{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
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
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">День/противоположная неделя не найдены</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("copy/{day_id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task CopyClassesToOppositeWeekAsync(
        [FromRoute(Name = "day_id")] Guid dayId,
        CancellationToken cancellationToken = default)
    {
        await service.CopyClassesToOppositeWeekAsync(dayId, cancellationToken);
    }

    /// <summary>
    ///     Удаление всех пар из дня
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешное удаление всех пар дня</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">Доступ запрещен</response>
    /// <response code="404">День не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("clear/day/{day_id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task ClearDayClassesAsync(
        [FromRoute(Name = "day_id")] Guid dayId,
        CancellationToken cancellationToken = default)
    {
        await service.ClearDayClassesAsync(dayId, cancellationToken);
    }

    /// <summary>
    ///     Удаление всех пар из расписания группы
    /// </summary>
    /// <param name="groupId">Идентификатор дня</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешное удаление всех пар группы</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">Доступ запрещен</response>
    /// <response code="404">День не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("clear/group/{group_id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task ClearWeeksClassesAsync(
        [FromRoute(Name = "group_id")] Guid groupId,
        CancellationToken cancellationToken = default)
    {
        await service.ClearWeeksClassesAsync(groupId, cancellationToken);
    }
    
    /// <summary>
    ///     Получение списка отмененных пар группы
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список отмененных пар</returns>
    /// <response code="200">Успешное удаление всех пар дня</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">Доступ запрещен</response>
    /// <response code="404">День не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("cancelled/{group_id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task<CollectionResult<Class>> GetCancelledClassesAsync(
        [FromRoute(Name = "group_id")] Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var classes = await service.GetCancelledClassesAsync(groupId, cancellationToken);

        return classes;
    }

    /// <summary>
    ///     Отмена пар по списку идентификаторов дня
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список отмененных пар</returns>
    /// <response code="200">Успешное удаление всех пар дня</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">Доступ запрещен</response>
    /// <response code="404">Не все дни найдены</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("cancel/days")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.Forbidden,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task CancelMultipleAsync(
        [FromBody] ClassMultipleCancelByDayIdParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await service.CancelMultipleAsync(parameters, cancellationToken);
    }

    /// <summary>
    ///     Отмена пар по дням недели и типу недели
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешная отмена пар</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("cancel/week-days")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task CancelAllByWeekDaysAsync(
        [FromBody] ClassCancelByWeekDaysParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await service.CancelAllByWeekDaysAsync(parameters, cancellationToken);
    }

    /// <summary>
    ///     Отмена всех пар для указанной группы
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешная отмена пар</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Группа не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("cancel/group/{groupId}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest, 
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task CancelMultipleByGroupAsync(
        [FromRoute] Guid groupId,
        CancellationToken cancellationToken = default)
    {
        await service.CancelMultipleByGroupAsync(groupId, cancellationToken);
    }

    /// <summary>
    ///     Отмена нескольких пар по их идентификаторам
    /// </summary>
    /// <param name="parameters">Параметры с идентификаторами пар</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешная отмена пар</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("cancel/multiple")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task CancelMultipleByIdsAsync(
        [FromBody] ClassMultipleCancelByIdParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await service.CancelMultipleAsync(parameters, cancellationToken);
    }
}