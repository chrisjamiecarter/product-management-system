using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Shared.Models;

/// <summary>
/// Helper class to manage validation-related functionality for models.
/// </summary>
internal static class ValidationHelper
{
    public static string GetValidationCssClass<TModel>(TModel model, string propertyName, bool showValidation = true)
    {
        return showValidation && HasValidationErrors(model, propertyName) ? "is-invalid" : "";
    }

    private static bool HasValidationErrors<TModel>(TModel model, string propertyName)
    {
        if (model == null) return false;

        var property = typeof(TModel).GetProperty(propertyName);
        if (property == null) throw new ArgumentException($"Property '{propertyName}' not found on type {typeof(TModel).Name}");

        var propertyValue = property.GetValue(model);
        var validationContext = new ValidationContext(model)
        {
            MemberName = propertyName
        };
        var validationResults = new List<ValidationResult>();
        var isInvalid = !Validator.TryValidateProperty(propertyValue, validationContext, validationResults);

        if (propertyValue == null && validationResults.Any(v => v.ErrorMessage?.Contains("required", StringComparison.OrdinalIgnoreCase) == false))
        {
            return false;
        }

        return isInvalid;
    }
}
