using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Errors;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Services;

internal class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IdentityOptions _options;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(ILogger<AuthService> logger, IOptions<IdentityOptions> options, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _options = options.Value;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Result> AddToRoleAsync(string userId, string? role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Success();
        }

        if (string.IsNullOrWhiteSpace(role))
        {
            return Result.Success();
        }

        var result = await _userManager.AddToRoleAsync(user, role);
        if (result.Succeeded)
        {
            return Result.Success();
        }
        else
        {
            var identityError = result.Errors.First();
            return identityError != null
                ? Result.Failure(new Error(identityError.Code, identityError.Description))
                : Result.Failure(UserErrors.NotAddedToRole);
        }
    }

    public async Task<Result> ConfirmEmailAsync(string userId, AuthToken token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        var result = await _userManager.ConfirmEmailAsync(user, token.Value);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogWarning("IdentityError during {method} for {userId}: {errorCode} - {errorDescription}", nameof(ConfirmEmailAsync), userId, error.Code, error.Description);
            }

            return Result.Failure(UserErrors.EmailConfirmedNotChanged);
        }

        return Result.Success();
    }

    public async Task<Result<AuthToken>> GenerateEmailChangeTokenAsync(string userId, string updatedEmail, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<AuthToken>(UserErrors.NotFound);
        }

        var token = AuthToken.Encode(await _userManager.GenerateChangeEmailTokenAsync(user, updatedEmail));

        return Result.Success(token);
    }

    public async Task<Result<AuthToken>> GenerateEmailConfirmationTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<AuthToken>(UserErrors.NotFound);
        }

        var token = AuthToken.Encode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
        return Result.Success(token);
    }

    public async Task<Result<AuthToken>> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<AuthToken>(UserErrors.NotFound);
        }

        var token = AuthToken.Encode(await _userManager.GeneratePasswordResetTokenAsync(user));
        return Result.Success(token);
    }

    public async Task<Result<ApplicationUserDto>> GetCurrentUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(UserErrors.NotFound);
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
            return Result.Failure(UserErrors.LockedOut);
        }

        if (result.IsNotAllowed)
        {
            return Result.Failure(UserErrors.NotAllowed);
        }

        if (result.RequiresTwoFactor)
        {
            return Result.Failure(UserErrors.RequiresTwoFactor);
        }

        return Result.Failure(UserErrors.InvalidSignInAttempt);
    }

    public async Task<Result> RefreshSignInAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        await _signInManager.RefreshSignInAsync(user);
        return Result.Success();
    }

    public async Task<Result> RegisterAsync(string email, string? password, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
        };

        var result = await (string.IsNullOrWhiteSpace(password) ? _userManager.CreateAsync(user) : _userManager.CreateAsync(user, password));
        if (result.Succeeded)
        {
            return Result.Success();
        }
        else
        {
            var identityError = result.Errors.First();
            return identityError != null
                ? Result.Failure(new Error(identityError.Code, identityError.Description))
                : Result.Failure(UserErrors.NotRegistered);
        }
    }

    public async Task<Result> ResetPasswordAsync(string email, string password, AuthToken token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            // Log the invalid attempt. But return successful.
            // AS to not reveal that the user does not exist.
            return Result.Success();
        }

        // TODO: Log better info here?
        var result = await _userManager.ResetPasswordAsync(user, token.Value, password);
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.ErrorResettingPassword);
    }

    public async Task<Result> SignInAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
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
        var user = await _userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (!_userManager.SupportsUserSecurityStamp)
        {
            return Result.Success();
        }

        var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
        var userStamp = await _userManager.GetSecurityStampAsync(user);

        return principalStamp == userStamp
            ? Result.Success()
            : Result.Failure(UserErrors.InvalidSecurityStamp);
    }
}
