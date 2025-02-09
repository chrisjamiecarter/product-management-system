using ProductManagement.Domain.Shared;

namespace ProductManagement.Infrastructure.Errors;

/// <summary>
/// Defines error codes and messages related to email operations.
/// </summary>
internal static class EmailErrors
{
    internal static readonly Error NotSent = new(
        "Email.NotSent",
        "There was an error when sending the email.");
}
