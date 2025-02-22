using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Abstractions.Entities;
using UniSchedule.Abstractions.Entities.Auditable;
using ValidationException = UniSchedule.Extensions.Exceptions.ValidationException;

namespace UniSchedule.Validation;

/// <summary>
///     Базовый валидатор
/// </summary>
/// <remarks>
///     Методы для валидации необходимо делать синхронными для работы автоматической валидации.
///     Конвейер зависимостей в ASP.NET работает синхронно, поэтому валидаторы с асинхронными методами будут падать с
///     исключениями.
/// </remarks>
/// <typeparam name="T">Тип валидируемого объекта</typeparam>
public abstract class ValidatorBase<T> : AbstractValidator<T>, IValidatorInterceptor
{
    private readonly DbContext _context;

    /// <summary />
    protected ValidatorBase(DbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public virtual IValidationContext BeforeAspNetValidation(
        ActionContext actionContext,
        IValidationContext commonContext)
    {
        return commonContext;
    }

    /// <inheritdoc />
    public ValidationResult AfterAspNetValidation(
        ActionContext actionContext,
        IValidationContext validationContext,
        ValidationResult result)
    {
        if (result.IsValid)
        {
            return result;
        }

        var exceptions = result.Errors.Select(e => new InvalidDataException(e.ErrorMessage));

        throw new ValidationException(exceptions);
    }


    /// <summary>
    ///     Проверка на существование сущности по ее идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <returns></returns>
    protected bool IsExist<TEntity, TKey>(TKey id)
        where TEntity : Entity<TKey>
    {
        return _context
            .Set<TEntity>()
            .AsNoTracking()
            .Any(e => e.Id!.Equals(id));
    }

    /// <summary>
    ///     Проверка на существование сущности по предикату
    /// </summary>
    /// <param name="predicate">Предикат</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <returns></returns>
    protected bool IsExist<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : Entity<TKey>
    {
        return _context
            .Set<TEntity>()
            .AsNoTracking()
            .Any(predicate);
    }

    /// <summary>
    ///     Проверка на существование списка сущностей по их идентификаторам
    /// </summary>
    /// <param name="ids">Список идентификаторов сущностей</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    protected bool IsExists<TEntity, TKey>(ICollection<TKey>? ids) where TEntity : Entity<TKey>
    {
        if (ids == null)
        {
            return true;
        }

        var entityIds = _context
            .Set<TEntity>()
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .Select(e => e.Id)
            .ToList();

        var excepted = ids.Except(entityIds);

        return !excepted.Any();
    }

    /// <summary>
    ///     Проверка на существование сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    protected bool IsExists<TEntity, TKey>(TKey id)
        where TEntity : Entity<TKey>
        where TKey : notnull
    {
        var isExists = _context
            .Set<TEntity>()
            .AsNoTracking()
            .Any(e => e.Id.Equals(id));

        return isExists;
    }

    /// <summary>
    ///     Проверка на существование сущности по предикату
    /// </summary>
    /// <param name="keySelector">Свойство сущности</param>
    /// <param name="value">Значение</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <typeparam name="TProperty">Тип свойства сущности</typeparam>
    /// <returns></returns>
    protected bool IsExist<TEntity, TKey, TProperty>(Func<TEntity, TProperty> keySelector, TProperty value)
        where TEntity : Entity<TKey>
    {
        var isExists = _context
            .Set<TEntity>()
            .Select(keySelector)
            .Any(e => e!.Equals(value));

        return isExists;
    }

    /// <summary>
    ///     Проверка, является ли сущность архивированной
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <returns></returns>
    protected bool IsNotArchived<TEntity, TKey>(TKey id)
        where TEntity : Entity<TKey>, IArchivable
    {
        var entity = _context
            .Set<TEntity>()
            .AsNoTracking()
            .SingleOrDefault(e => e.Id!.Equals(id));

        return entity == null || !entity.IsArchived();
    }

    /// <summary>
    ///     Получает значение поля сущности по названию
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="propertyName">Название поля</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <returns>Значение поля сущности</returns>
    protected object GetPropertyValue<TEntity, TKey>(TKey id, string propertyName)
        where TEntity : Entity<TKey>
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        var entity = _context
            .Set<TEntity>()
            .AsNoTracking()
            .SingleOrDefault(e => e.Id!.Equals(id));

        if (entity == null)
        {
            throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} with ID {id} not found.");
        }

        var propertyInfo = typeof(TEntity).GetProperty(propertyName.Dehumanize());
        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property {propertyName} not found on type {typeof(TEntity).FullName}");
        }

        return propertyInfo.GetValue(entity, null)!;
    }
}