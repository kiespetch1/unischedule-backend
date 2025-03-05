namespace UniSchedule.Abstractions.Helpers.Database;

/// <summary>
///     Функционал инициализации данных
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    ///     Инициализация данных
    /// </summary>
    Task SeedAsync();
}