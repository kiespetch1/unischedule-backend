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
///     Контроллер для работы с группами
/// </summary>
[ApiController]
[Route("/api/v1/[controller]")]
public class GroupsController(
    ICreateCommand<Group, GroupCreateParameters, Guid> create,
    IDeleteCommand<Group, Guid> delete,
    IUpdateCommand<Group, GroupUpdateParameters, Guid> update,
    IMultipleQuery<Group, GroupQueryParameters> get,
    ISingleQuery<Group, Guid> getById,
    IGroupService service,
    IMapper mapper)
{
    /// <summary>
    ///     Создание группы
    /// </summary>
    /// <param name="parameters">Параметры создания группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор группы</returns>
    /// <response code="200">Успешное создание группы</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPost]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin)]
    public async Task<Result<Guid>> CreateAsync(
        [FromBody] GroupCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var id = await create.ExecuteAsync(parameters, cancellationToken);
        return new Result<Guid>(id);
    }

    /// <summary>
    ///     Получение группы по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Группа</returns>
    /// <response code="200">Успешное получение группы</response>
    /// <response code="404">Группа не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<GroupModel>> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var group = await getById.ExecuteAsync(id, cancellationToken);
        var result = mapper.Map<GroupModel>(group);

        return new Result<GroupModel>(result);
    }

    /// <summary>
    ///     Получение списка групп
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список групп</returns>
    /// <response code="200">Успешное получение групп</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<CollectionResult<GroupModel>> GetAsync(
        [FromQuery] GroupQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var groups = await get.ExecuteAsync(parameters, cancellationToken);
        var data = mapper.Map<CollectionResult<GroupModel>>(groups);

        return data;
    }

    /// <summary>
    ///     Обновление группы
    /// </summary>
    /// <param name="id">Идентификатор группы</param>
    /// <param name="parameters">Параметры обновления группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное обновление группы</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Группа не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPut("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin)]
    public async Task UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] GroupUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await update.ExecuteAsync(id, parameters, cancellationToken);
    }

    /// <summary>
    ///     Удаление группы
    /// </summary>
    /// <param name="id">Идентификатор группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное удаление группы</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Группа не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpDelete("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin)]
    public async Task DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await delete.ExecuteAsync(id, cancellationToken);
    }

    /// <summary>
    ///     Переход в следующий курс для всех групп
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное обновление курса групп</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPatch("promote")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin)]
    public async Task PromoteGroupsAsync(CancellationToken cancellationToken = default)
    {
        await service.PromoteGroupsAsync(cancellationToken);
    }

    /// <summary>
    ///     Импорт расписания пар для группы с официального сайта
    /// </summary>
    /// <param name="parameters">Параметры импорта расписания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">Успешный импорт расписания</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Группа не найдена</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpPut("import-classes")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    [Authorize(RoleOption.Admin, RoleOption.GroupLeader, RoleOption.Staff)]
    public async Task ImportClassesScheduleAsync(
        [FromBody] ClassScheduleImportParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await service.ImportClassesScheduleAsync(parameters, cancellationToken);
    }
}