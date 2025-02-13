using Microsoft.AspNetCore.Components.Forms;

namespace ProductManagement.BlazorApp.Components.Shared.Models;

/// <summary>
/// Custom input select component that formats nullable boolean values correctly for display.
/// </summary>
/// <typeparam name="TValue">The type of value bound to the input.</typeparam>
public class FixedInputSelect<TValue> : InputSelect<TValue>
{
    protected override string? FormatValueAsString(TValue? value)
    {
        if (typeof(TValue) == typeof(bool))
        {
            return (bool)(object)value! ? "true" : "false";
        }
        else if (typeof(TValue) == typeof(bool?))
        {
            if (value == null)
                return null;
            else
                return (bool)(object)value ? "true" : "false";
        }

        return base.FormatValueAsString(value);
    }
}
