using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UniSchedule.Extensions.DI.Controllers;

/// <inheritdoc cref="IValueProviderFactory" />
public class SnakeCaseQueryValueProviderFactory : IValueProviderFactory
{
    /// <inheritdoc />
    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var valueProvider = new SnakeCaseQueryValueProvider(
            BindingSource.Query,
            context.ActionContext.HttpContext.Request.Query,
            CultureInfo.CurrentCulture);

        context.ValueProviders.Add(valueProvider);

        return Task.CompletedTask;
    }
}