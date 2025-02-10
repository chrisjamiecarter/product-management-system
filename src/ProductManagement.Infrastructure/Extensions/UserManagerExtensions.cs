using Microsoft.AspNetCore.Identity;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Extensions;

internal static class UserManagerExtensions
{
    /// <summary>
    /// Adds the <paramref name="password"/> to the specified <paramref name="user"/> only if the user
    /// does not already have a password.
    /// </summary>
    /// <param name="user">The user whose password should be set.</param>
    /// <param name="password">The password to set.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> AddPasswordAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
    {
        var result = await userManager.AddPasswordAsync(user, password);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Add the specified <paramref name="user"/> to the named role.
    /// </summary>
    /// <param name="user">The user to add to the named role.</param>
    /// <param name="role">The name of the role to add the user to.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    /// <remarks>
    /// If the named role is null or white space, then a Success <see cref="Result"/> is returned straight away.
    /// </remarks>
    public static async Task<Result> AddToRoleAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return Result.Success();
        }

        var result = await userManager.AddToRoleAsync(user, role);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Updates a users emails if the specified email change <paramref name="token"/> is valid for the user.
    /// </summary>
    /// <param name="user">The user whose email should be updated.</param>
    /// <param name="newEmail">The new email address.</param>
    /// <param name="token">The change email token to be verified.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> ChangeEmailAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string newEmail, string token)
    {
        var result = await userManager.ChangeEmailAsync(user, newEmail, token);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Changes a user's password after confirming the specified <paramref name="currentPassword"/> is correct,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose password should be set.</param>
    /// <param name="currentPassword">The current password to validate before changing.</param>
    /// <param name="newPassword">The new password to set for the specified <paramref name="user"/>.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> ChangePasswordAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string currentPassword, string newPassword)
    {
        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Validates that an email confirmation token matches the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to validate the token against.</param>
    /// <param name="token">The email confirmation token to validate.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> ConfirmEmailAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string token)
    {
        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the backing store with no password,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> CreateAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        var result = await userManager.CreateAsync(user);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the backing store with given password,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="password">The password for the user to hash and store.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> CreateAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Deletes the specified <paramref name="user"/> from the backing store.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> DeleteAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        var result = await userManager.DeleteAsync(user);
        return result.ToDomainResult();
    }


    /// <summary>
    /// Removes the specified <paramref name="user"/> from the named role.
    /// </summary>
    /// <param name="user">The user to remove from the named role.</param>
    /// <param name="role">The name of the role to remove the user from.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    /// <remarks>
    /// If the named role is null or white space, then a Success <see cref="Result"/> is returned straight away.
    /// </remarks>
    public static async Task<Result> RemoveFromRoleAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return Result.Success();
        }

        var result = await userManager.RemoveFromRoleAsync(user, role);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Removes the specified <paramref name="user"/> from the named roles.
    /// </summary>
    /// <param name="user">The user to remove from the named roles.</param>
    /// <param name="roles">The name of the roles to remove the user from.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> RemoveFromRolesAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, IEnumerable<string> roles)
    {
        var result = await userManager.RemoveFromRolesAsync(user, roles);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Resets the <paramref name="user"/>'s password to the specified <paramref name="newPassword"/> after
    /// validating the given password reset <paramref name="token"/>.
    /// </summary>
    /// <param name="user">The user whose password should be reset.</param>
    /// <param name="token">The password reset token to verify.</param>
    /// <param name="newPassword">The new password to set if reset token verification succeeds.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> ResetPasswordAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string token, string newPassword)
    {
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Sets the given <paramref name="userName" /> for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="userName">The user name to set.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public static async Task<Result> SetUserNameAndReturnDomainResultAsync(this UserManager<ApplicationUser> userManager, ApplicationUser user, string? userName)
    {
        var result = await userManager.SetUserNameAsync(user, userName);
        return result.ToDomainResult();
    }
}
