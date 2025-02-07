namespace UniSchedule.Extensions.Helpers;

/// <summary>
///     Функционал запуска миграций БД
/// </summary>
public interface IMigrationDatabase
{
    /// <summary>
    ///     Применить миграции
    /// </summary>
    Task MigrateAsync();
}