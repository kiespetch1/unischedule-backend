using System.Reflection;
using UniSchedule.Abstractions.Entities;
using UniSchedule.Extensions.Attributes;
using UniSchedule.Extensions.Basic;

namespace UniSchedule.Identity.Entities;

/// <summary>
///     Запись в справочнике
/// </summary>
public abstract class HandbookEntry : Entity<Guid>
{
    /// <summary>
    ///     Код
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    ///     Название
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    ///     Описание
    /// </summary>
    public string Description { get; set; }
}

public abstract class HandbookEntry<TCode> : HandbookEntry where TCode : struct, Enum
{
    protected internal HandbookEntry(TCode code)
    {
        var info = code
            .GetType()
            .GetRuntimeField(code.ToString())
            ?.GetCustomAttribute<HandbookValueAttribute>();

        Id = info?.Id ?? Guid.NewGuid();
        Code = code.ToInt32();
        Alias = code.ToString();
        Description = info?.Description ?? code.ToString();
    }
}