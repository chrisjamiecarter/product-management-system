using Microsoft.AspNetCore.Identity;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for converting an IdentityResult to a domain <see cref="Result"/>.
/// </summary>
internal static class IdentityResultExtensions
{
    private static readonly string Code = "Identity.Error";

    public static Result ToDomainResult(this IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
        {
            return Result.Success();
        }

        var message = string.Join(' ', identityResult.Errors.Select(e => e.Description));
        return Result.Failure(new Error(Code, message));
    }
}
