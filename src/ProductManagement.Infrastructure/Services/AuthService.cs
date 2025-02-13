using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Interfaces;
using ProductManagement.Infrastructure.Models;
using static ProductManagement.Application.Errors.ApplicationErrors;

namespace ProductManagement.Infrastructure.Services;

/// <summary>
/// Provides the service for authentication operations.
/// </summary>
internal class AuthService : IAuthService
{
    private readonly IdentityOptions _options;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserManagerWrapper _userManagerWrapper;

    public AuthService(IOptions<IdentityOptions> options, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserManagerWrapper userManagerWrapper)
    {
        _options = options.Value;
        _signInManager = signInManager;
        _userManager = userManager;
        _userManagerWrapper = userManagerWrapper;
    }

    public async Task<Result> ConfirmEmailAsync(string userId, AuthToken token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        var result = await _userManagerWrapper.ConfirmEmailAndReturnDomainResultAsync(user, token.Value);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result<AuthToken>> GenerateEmailChangeTokenAsync(string userId, string updatedEmail, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<AuthToken>(User.NotFound(userId));
        }

        var token = AuthToken.Encode(await _userManager.GenerateChangeEmailTokenAsync(user, updatedEmail));

        return Result.Success(token);
    }

    public async Task<Result<AuthToken>> GenerateEmailConfirmationTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<AuthToken>(User.EmailNotFound(email));
        }

        var token = AuthToken.Encode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
        return Result.Success(token);
    }

    public async Task<Result<AuthToken>> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<AuthToken>(User.EmailNotFound(email));
        }

        var token = AuthToken.Encode(await _userManager.GeneratePasswordResetTokenAsync(user));
        return Result.Success(token);
    }

    public async Task<Result<ApplicationUserDto>> GetCurrentUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(User.NotFound(userId));
        }

        var roles = await _userManager.GetRolesAsync(user);

        var response = new ApplicationUserDto(user.Id, user.Email, user.EmailConfirmed, roles.FirstOrDefault());
        return Result.Success(response);
    }

    public async Task<Result> PasswordSignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, remember, false);

        if (result.Succeeded)
        {
            return Result.Success();
        }

        if (result.IsLockedOut)
        {
            return Result.Failure(User.LockedOut(email));
        }

        if (result.IsNotAllowed)
        {
            return Result.Failure(User.NotAllowed(email));
        }

        if (result.RequiresTwoFactor)
        {
            return Result.Failure(User.RequiresTwoFactor(email));
        }

        return Result.Failure(User.InvalidSignInAttempt(email));
    }

    public async Task<Result> RefreshSignInAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        await _signInManager.RefreshSignInAsync(user);
        return Result.Success();
    }

    public async Task<Result> RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
        };

        var result = await _userManagerWrapper.CreateAndReturnDomainResultAsync(user, password);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(string email, string password, AuthToken token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure(User.EmailNotFound(email));
        }

        var result = await _userManagerWrapper.ResetPasswordAndReturnDomainResultAsync(user, token.Value, password);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result> SignInAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        await _signInManager.SignInAsync(user, false);
        return Result.Success();
    }

    public async Task<Result> SignOutAsync(CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();
        return Result.Success();
    }

    public async Task<Result> ValidateSecurityStampAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var userId = principal.FindFirstValue(_options.ClaimsIdentity.UserIdClaimType) ?? string.Empty;

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        if (!_userManager.SupportsUserSecurityStamp)
        {
            return Result.Success();
        }

        var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
        var userStamp = await _userManager.GetSecurityStampAsync(user);

        return principalStamp == userStamp
            ? Result.Success()
            : Result.Failure(User.InvalidSecurityStamp(userId));
    }
}
