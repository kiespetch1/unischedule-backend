using UniSchedule.Abstractions.Commands;
using UniSchedule.Extensions.Collections;
using UniSchedule.Schedule.Database;
using UniSchedule.Schedule.Entities;
using UniSchedule.Shared.DTO.Parameters;

namespace UniSchedule.Schedule.Commands;

/// <summary>
///     Команды для работы с преподавателями
/// </summary>
public class TeacherCommands(DatabaseContext context) :
    ICreateCommand<Teacher, TeacherCreateParameters, Guid>,
    IUpdateCommand<Teacher, TeacherUpdateParameters, Guid>,
    IDeleteCommand<Teacher, Guid>
{
    /// <summary>
    ///     Создание преподавателя
    /// </summary>
    /// <param name="parameters">Параметры создания преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданного преподавателя</returns>
    public async Task<Guid> ExecuteAsync(
        TeacherCreateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var teacher = new Teacher { Name = parameters.Name, FullName = parameters.FullName };

        context.Teachers.Add(teacher);
        await context.SaveChangesAsync(cancellationToken);

        return teacher.Id;
    }

    /// <summary>
    ///     Обновление преподавателя
    /// </summary>
    /// <param name="id">Идентификатор преподавателя</param>
    /// <param name="parameters">Параметры обновления преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(
        Guid id,
        TeacherUpdateParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var teacher = await context.Teachers.SingleOrNotFoundAsync(id, cancellationToken);

        teacher.Name = parameters.Name;
        teacher.FullName = parameters.FullName;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Удаление преподавателя
    /// </summary>
    /// <param name="id">Идентификатор преподавателя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var teacher = await context.Teachers.SingleOrNotFoundAsync(id, cancellationToken);
        // TODO: к моменту когда будет необходимо удаление, добавить инклюд для удаления с другими сущностями, и предупреждение о этом

        context.Teachers.Remove(teacher);
        await context.SaveChangesAsync(cancellationToken);
    }
}