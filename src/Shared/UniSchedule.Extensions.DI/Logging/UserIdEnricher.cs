using Serilog.Core;
using Serilog.Events;
using UniSchedule.Abstractions.Helpers.Identity;

namespace UniSchedule.Extensions.DI.Logging;

/// <summary>
///     Enricher для идентификатора пользователя
/// </summary>
public class UserIdEnricher(IUserContextProvider userContextProvider) : ILogEventEnricher
{
    /// <inheritdoc cref="ILogEventEnricher.Enrich" />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var userId = userContextProvider.GetContext().Id;

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("UserId", userId));
    }
}