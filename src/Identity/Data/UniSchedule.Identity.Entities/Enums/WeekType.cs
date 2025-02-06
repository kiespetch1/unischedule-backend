using System.ComponentModel;

namespace UniSchedule.Identity.Entities.Enums;

/// <summary>
///     Тип недели
/// </summary>
public enum WeekType
{
    /// <summary>
    ///     Все
    /// </summary>
    Every = 0,
    
    /// <summary>
    ///     Четная (верхняя)
    /// </summary>
    Even = 1,
    
    /// <summary>
    ///     Нечетная (нижняя)
    /// </summary>
    Odd = 2
}