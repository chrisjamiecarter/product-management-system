using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace ProductManagement.BlazorApp.Extensions;

/// <summary>
/// Extension methods for working with EditContext in Blazor.
/// </summary>
public static class EditContextExtensions
{
    public static string GetValidationClass<TValue>(this EditContext editContext, Expression<Func<TValue>> propertyExpression)
    {
        var fieldName = propertyExpression.Body is MemberExpression member ? member.Member.Name : string.Empty;
        var fieldIdentifier = editContext.Field(fieldName);
        
        return editContext.IsValid(fieldIdentifier)
            ? string.Empty
            : "is-invalid";
    }
}
