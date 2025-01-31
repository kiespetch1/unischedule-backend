using System.Globalization;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UniSchedule.Extensions.DI.Controllers;

/// <inheritdoc cref="QueryStringValueProvider" />
public class SnakeCaseQueryValueProvider : QueryStringValueProvider
{
    /// <summary />
    public SnakeCaseQueryValueProvider(
        BindingSource bindingSource,
        IQueryCollection values,
        CultureInfo culture)
        : base(bindingSource, values, culture)
    {
    }

    /// <inheritdoc />
    public override bool ContainsPrefix(string prefix)
    {
        return base.ContainsPrefix(prefix.Underscore());
    }

    /// <inheritdoc />
    public override ValueProviderResult GetValue(string key)
    {
        return base.GetValue(key.Underscore());
    }
}