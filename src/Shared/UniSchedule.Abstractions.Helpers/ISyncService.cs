namespace UniSchedule.Abstractions.Helpers;

/// <summary>
///     Сервис для синхронизации сущностей
/// </summary>
public interface ISyncService
{
    /// <summary>
    ///     Запуск синхронизации сущностей
    /// </summary>
    Task SyncAsync();
}