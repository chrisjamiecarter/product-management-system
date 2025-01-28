using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Models;
using ProductManagement.Infrastructure.Errors;

namespace ProductManagement.Infrastructure.Services;

internal class AuthService : IAuthService
{
    private readonly IEmailSender<ApplicationUser> _emailSender;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(IEmailSender<ApplicationUser> emailSender, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _emailSender = emailSender;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Result> AddToRoleAsync(string email, string? role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
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

    public async Task<Result> ChangePasswordAsync(ClaimsPrincipal principal, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Result.Success();
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
        {
            // TODO: var passwordErrorMessage = $"Error: {string.Join(",", changePasswordResult.Errors.Select(error => error.Description))}";
            return Result.Failure(UserErrors.PasswordNotChanged);
        }

        await _signInManager.RefreshSignInAsync(user);
        return Result.Success();
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.EmailConfirmedNotChanged);
    }

    public async Task<Result> ForgotPasswordAsync(string email, string resetUrl, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (!user.EmailConfirmed)
        {
            return Result.Failure(UserErrors.EmailNotConfirmed);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var uriBuilder = new UriBuilder(resetUrl)
        {
            Query = $"code={code}"
        };
        var resetLink = HtmlEncoder.Default.Encode(uriBuilder.ToString());
        
        await _emailSender.SendPasswordResetLinkAsync(user, email, resetLink);
        return Result.Success();
    }

    public async Task<Result> GenerateEmailChangeAsync(ClaimsPrincipal principal, string email, string confirmUrl, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Result.Success();
        }

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, email);
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var uriBuilder = new UriBuilder(confirmUrl)
        {
            Query = $"userId={user.Id}&email={email}&code={code}"
        };
        var confirmationLink = HtmlEncoder.Default.Encode(uriBuilder.ToString());

        await _emailSender.SendConfirmationLinkAsync(user, email, confirmationLink);
        return Result.Success();
    }

    public async Task<Result> GenerateEmailConfirmationAsync(string email, string confirmUrl, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Success();
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var uriBuilder = new UriBuilder(confirmUrl)
        {
            Query = $"userId={user.Id}&code={code}"
        };
        var confirmationLink = HtmlEncoder.Default.Encode(uriBuilder.ToString());

        await _emailSender.SendConfirmationLinkAsync(user, email, confirmationLink);
        return Result.Success();
    }

    public async Task<Result<ApplicationUserDto>> GetCurrentUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(UserErrors.NotFound);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var response = new ApplicationUserDto(user.Id, user.UserName, roles.FirstOrDefault(), user.EmailConfirmed);
        return Result.Success(response);
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

    public async Task<Result> ResetPasswordAsync(string email, string password, string token, CancellationToken cancellationToken = default)
    {
        // TODO: 
        // Decide whether to pass the code around and decode centrally,
        // or
        // Decode the code before passing it to this method.
        // Thinking Token token = Token.Encode(token); and Token.Decode(code);
        // Token has a Value property that is always the decoded value.
        // Token has a Code property that is always the encoded value.

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            // Log the invalid attempt. But return successful.
            // AS to not reveal that the user does not exist.
            return Result.Success();
        }

        // TODO: Log better info here?
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.ErrorResettingPassword);
    }

    public async Task<Result> SignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default)
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

    public async Task<Result> SignOutAsync(CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();
        return Result.Success();
    }
}
