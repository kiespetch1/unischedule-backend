namespace UniSchedule.Extensions.Attributes;

/// <summary>
///     Атрибут для построения значения справочника из значения <see cref="Enum" />
/// </summary>
/// <param name="id">Идентификатор в строковом формате</param>
/// <param name="description">Описание</param>
[AttributeUsage(AttributeTargets.Field)]
public class HandbookValueAttribute(string id, string description) : Attribute
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; } = Guid.Parse(id);

    /// <summary>
    ///     Описание
    /// </summary>
    public string Description { get; } = description;
}