using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Shared.Models;

internal static class ValidationHelper
{
    public static string GetValidationCssClass<TModel>(TModel model, string propertyName, bool showValidation = true)
    {
        return showValidation && HasValidationErrors(model, propertyName) ? "is-invalid" : "";
    }

    private static bool HasValidationErrors<TModel>(TModel model, string propertyName)
    {
        // if (model == null) throw new ArgumentNullException(nameof(model));
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

        // Assume valid for null values unless explicitly required.
        if (propertyValue == null && validationResults.Any(v => v.ErrorMessage?.Contains("required", StringComparison.OrdinalIgnoreCase) == false))
        {
            return false;
        }

        return isInvalid;

        //return !Validator.TryValidateProperty(propertyValue, validationContext, validationResults);
    }
}
