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
using UniSchedule.Shared.DTO.Models;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Api.Controllers;

/// <summary>
///     Контроллер для работы с преподавателями
/// </summary>
[ApiController]
[Route("/api/v1/[controller]")]
public class TeachersController(
    ICreateCommand<Teacher, TeacherCreateParameters, Guid> create,
    IDeleteCommand<Teacher, Guid> delete,
    IUpdateCommand<Teacher, TeacherUpdateParameters, Guid> update,
    IMultipleQuery<Teacher, TeacherQueryParameters> get,
    ISingleQuery<Teacher, Guid> getById,
    IMapper mapper)
{
    /// <summary>
    ///     Создание преподавателя
    /// </summary>
    /// <param name="parameters">Параметры создания преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор преподавателя</returns>
    /// <response code="200">Успешное создание преподавателя</response>
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
        [FromBody] TeacherCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var id = await create.ExecuteAsync(parameters, cancellationToken);

        return new Result<Guid>(id);
    }

    /// <summary>
    ///     Получение преподавателя по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Преподаватель</returns>
    /// <response code="200">Успешное получение преподавателя</response>
    /// <response code="404">Преподаватель не найден</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet("{id}")]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.NotFound,
        HttpStatusCode.InternalServerError)]
    public async Task<Result<TeacherModel>> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var teacher = await getById.ExecuteAsync(id, cancellationToken);
        var result = mapper.Map<TeacherModel>(teacher);

        return new Result<TeacherModel>(result);
    }

    /// <summary>
    ///     Получение списка преподавателей
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список преподавателей</returns>
    /// <response code="200">Успешное получение преподавателей</response>
    /// <response code="500">Непредвиденная ошибка</response>
    [HttpGet]
    [ResponseStatusCodes(
        HttpStatusCode.OK,
        HttpStatusCode.InternalServerError)]
    public async Task<CollectionResult<TeacherModel>> GetAsync(
        [FromQuery] TeacherQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var teachers = await get.ExecuteAsync(parameters, cancellationToken);
        var data = mapper.Map<CollectionResult<TeacherModel>>(teachers);

        return data;
    }

    /// <summary>
    ///     Обновление преподавателя
    /// </summary>
    /// <param name="id">Идентификатор преподавателя</param>
    /// <param name="parameters">Параметры обновления преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное обновление преподавателя</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Преподаватель не найден</response>
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
        [FromBody] TeacherUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        await update.ExecuteAsync(id, parameters, cancellationToken);
    }

    /// <summary>
    ///     Удаление преподавателя
    /// </summary>
    /// <param name="id">Идентификатор преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Успешное удаление преподавателя</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Преподаватель не найден</response>
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
}