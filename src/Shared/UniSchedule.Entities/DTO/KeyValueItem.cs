namespace UniSchedule.Entities.DTO;

/// <summary>
///     Модель ключ-значение для передачи на клиент
/// </summary>
public class KeyValueItem<TKey>
{
    /// <summary />
    public KeyValueItem(TKey id, string displayName)
    {
        Id = id;
        DisplayName = displayName;
    }

    /// <summary />
    public KeyValueItem(TKey id)
    {
        Id = id;
        DisplayName = id?.ToString() ?? string.Empty;
    }

    /// <summary>
    ///     Идентификатор
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    ///     Отображаемое наименование
    /// </summary>
    public string DisplayName { get; set; }
}
