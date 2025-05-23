using System.ComponentModel;
using System.Reflection;
using UniSchedule.Extensions.Attributes;
using System.Runtime.Serialization;


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
    ///     Получение значения идентификатора из атрибута <see cref="HandbookValueAttribute" />.
    /// </summary>
    /// <param name="source">Значение Enum</param>
    /// <returns>Текстовое значение идентификатора из атрибута <see cref="HandbookValueAttribute" /></returns>
    public static Guid GetId(this Enum source)
    {
        var attribute = source
            .GetType()
            .GetRuntimeField(source.ToString())
            ?.GetCustomAttribute<HandbookValueAttribute>();

        return attribute?.Id ?? Guid.Empty;
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

    /// <summary>
    ///     Проверка наличия значения в перечислении
    /// </summary>
    /// <param name="value">Проверяемое значениe</param>
    /// <typeparam name="TEnum">Тип перечиcления</typeparam>
    /// <returns>true, если значение присутствует в перечислении; false, в противном случае</returns>
    public static bool IsValidEnum<TEnum>(TEnum value)
    {
        return value != null && Enum.IsDefined(typeof(TEnum), value);
    }
    
    /// <summary>
    ///     Получение имени с атрибута <see cref="EnumMemberAttribute"/>
    /// </summary>
    /// <param name="value">Проверяемое значение</param>
    /// <returns></returns>
    public static string GetMemberValue(this Enum value)
    {
        var type  = value.GetType();
        var name  = Enum.GetName(type, value);
        if (name is null)
        {
            return value.ToString();
        }

        var field = type.GetField(name);
        var attr  = field?
            .GetCustomAttribute<EnumMemberAttribute>(false);

        return attr?.Value ?? name;
    }
}