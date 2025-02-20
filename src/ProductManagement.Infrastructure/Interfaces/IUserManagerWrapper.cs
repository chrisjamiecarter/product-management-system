using Microsoft.AspNetCore.Identity;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Interfaces;

/// <summary>
/// Defines the wrapper for the UserManager class. Handles returning a domain <see cref="Result"/> instead of an IdentityResult.
/// </summary>
internal interface IUserManagerWrapper
{
    Task<Result> AddLoginAndReturnDomainResultAsync(ApplicationUser user, UserLoginInfo login);
    Task<Result> AddPasswordAndReturnDomainResultAsync(ApplicationUser user, string password);
    Task<Result> AddToRoleAndReturnDomainResultAsync(ApplicationUser user, string? role);
    Task<Result> ChangeEmailAndReturnDomainResultAsync(ApplicationUser user, string newEmail, string token);
    Task<Result> ChangePasswordAndReturnDomainResultAsync(ApplicationUser user, string currentPassword, string newPassword);
    Task<Result> ConfirmEmailAndReturnDomainResultAsync(ApplicationUser user, string token);
    Task<Result> CreateAndReturnDomainResultAsync(ApplicationUser user);
    Task<Result> CreateAndReturnDomainResultAsync(ApplicationUser user, string password);
    Task<Result> DeleteAndReturnDomainResultAsync(ApplicationUser user);
    Task<Result> RemoveFromRoleAndReturnDomainResultAsync(ApplicationUser user, string? role);
    Task<Result> RemoveFromRolesAndReturnDomainResultAsync(ApplicationUser user, IEnumerable<string> roles);
    Task<Result> RemoveLoginAndReturnDomainResultAsync(ApplicationUser user, string loginProvider, string providerKey);
    Task<Result> ResetPasswordAndReturnDomainResultAsync(ApplicationUser user, string token, string newPassword);
    Task<Result> SetUserNameAndReturnDomainResultAsync(ApplicationUser user, string? userName);
}