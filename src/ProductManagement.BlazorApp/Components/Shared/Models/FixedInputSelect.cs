using Microsoft.AspNetCore.Components.Forms;

namespace ProductManagement.BlazorApp.Components.Shared.Models;

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
