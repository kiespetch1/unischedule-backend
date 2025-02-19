namespace UniSchedule.Abstractions.Messaging;

/// <summary>
///     Функционал для добавления сообщений в очередь
/// </summary>
/// <typeparam name="TMessage">Тип сообщения</typeparam>
public interface IPublisher<in TMessage> where TMessage : class
{
    /// <summary>
    ///     Добавить сообщение в очередь
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task PublishAsync(TMessage message, CancellationToken cancellationToken = default);
}