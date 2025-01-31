using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Models;
using ProductManagement.Infrastructure.Errors;

namespace ProductManagement.Infrastructure.Services;

internal class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
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
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.EmailConfirmedNotChanged);
    }

    public async Task<Result<AuthToken>> GenerateEmailChangeTokenAsync(string userId, string newEmail, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<AuthToken>(UserErrors.NotFound);
        }

        var token = AuthToken.Encode(await _userManager.GenerateChangeEmailTokenAsync(user, newEmail));

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

        var response = new ApplicationUserDto(user.Id, user.UserName, roles.FirstOrDefault(), user.EmailConfirmed);
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
            UserName = email,
            Email = email,
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
}
