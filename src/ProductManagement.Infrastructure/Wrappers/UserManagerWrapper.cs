using Microsoft.AspNetCore.Identity;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Extensions;
using ProductManagement.Infrastructure.Interfaces;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Wrappers;

/// <summary>
/// Provides the wrapper for the UserManager class. Handles returning a domain <see cref="Result"/> instead of an IdentityResult.
/// </summary>
internal class UserManagerWrapper : IUserManagerWrapper
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerWrapper(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Adds an external <see cref="UserLoginInfo"/> to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the login to.</param>
    /// <param name="login">The external <see cref="UserLoginInfo"/> to add to the specified <paramref name="user"/>.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public async Task<Result> AddLoginAndReturnDomainResultAsync(ApplicationUser user, UserLoginInfo login)
    {
        var result = await _userManager.AddLoginAsync(user, login);
        return result.ToDomainResult();
    }

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
    public async Task<Result> AddPasswordAndReturnDomainResultAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.AddPasswordAsync(user, password);
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
    public async Task<Result> AddToRoleAndReturnDomainResultAsync(ApplicationUser user, string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return Result.Success();
        }

        var result = await _userManager.AddToRoleAsync(user, role);
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
    public async Task<Result> ChangeEmailAndReturnDomainResultAsync(ApplicationUser user, string newEmail, string token)
    {
        var result = await _userManager.ChangeEmailAsync(user, newEmail, token);
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
    public async Task<Result> ChangePasswordAndReturnDomainResultAsync(ApplicationUser user, string currentPassword, string newPassword)
    {
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
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
    public async Task<Result> ConfirmEmailAndReturnDomainResultAsync(ApplicationUser user, string token)
    {
        var result = await _userManager.ConfirmEmailAsync(user, token);
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
    public async Task<Result> CreateAndReturnDomainResultAsync(ApplicationUser user)
    {
        var result = await _userManager.CreateAsync(user);
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
    public async Task<Result> CreateAndReturnDomainResultAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
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
    public async Task<Result> DeleteAndReturnDomainResultAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);
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
    public async Task<Result> RemoveFromRoleAndReturnDomainResultAsync(ApplicationUser user, string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return Result.Success();
        }

        var result = await _userManager.RemoveFromRoleAsync(user, role);
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
    public async Task<Result> RemoveFromRolesAndReturnDomainResultAsync(ApplicationUser user, IEnumerable<string> roles)
    {
        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        return result.ToDomainResult();
    }

    /// <summary>
    /// Attempts to remove the provided external login information from the specified <paramref name="user"/>.
    /// and returns a flag indicating whether the removal succeed or not.
    /// </summary>
    /// <param name="user">The user to remove the login information from.</param>
    /// <param name="loginProvider">The login provide whose information should be removed.</param>
    /// <param name="providerKey">The key given by the external login provider for the specified user.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="Result"/>
    /// of the operation, which has been mapped from the <see cref="IdentityResult"/>.
    /// </returns>
    public async Task<Result> RemoveLoginAndReturnDomainResultAsync(ApplicationUser user, string loginProvider, string providerKey)
    {
        var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
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
    public async Task<Result> ResetPasswordAndReturnDomainResultAsync(ApplicationUser user, string token, string newPassword)
    {
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
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
    public async Task<Result> SetUserNameAndReturnDomainResultAsync(ApplicationUser user, string? userName)
    {
        var result = await _userManager.SetUserNameAsync(user, userName);
        return result.ToDomainResult();
    }

}
