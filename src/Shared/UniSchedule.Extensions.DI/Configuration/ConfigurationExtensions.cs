using Microsoft.Extensions.Configuration;

namespace UniSchedule.Extensions.DI.Configuration;

/// <summary>
///     Методы расширения для <see cref="IConfiguration" />
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Получение объекта из конфигурации
    /// </summary>
    /// <param name="configuration">
    ///     <see cref="IConfiguration" />
    /// </param>
    /// <typeparam name="T">Тип получаемого объекта</typeparam>
    /// <returns>Объект</returns>
    public static T GetSectionAs<T>(this IConfiguration configuration)
    {
        return configuration.GetSection(typeof(T).Name).Get<T>() ?? throw new ArgumentNullException(typeof(T).Name);
    }
}