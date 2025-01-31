﻿using System.ComponentModel;
using System.Reflection;

namespace UniSchedule.Extensions.Basic;

/// <summary>
///     Методы расширения для <see cref="Enum" />
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Получение значения из атрибута <see cref="DescriptionAttribute" />.
    /// </summary>
    /// <param name="source">Значение Enum</param>
    /// <returns>Текстовое значение из атрибута <see cref="DescriptionAttribute" /></returns>
    public static string GetDescription(this Enum source)
    {
        var attribute = source
            .GetType()
            .GetRuntimeField(source.ToString())
            ?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? source.ToString();
    }

    /// <summary>
    ///     Получение численного значения константы из перечисления
    /// </summary>
    /// <param name="source">Значение Enum</param>
    /// <returns>Численное значение константы</returns>
    public static int ToInt32(this Enum source)
    {
        return (int)(ValueType)source;
    }
}