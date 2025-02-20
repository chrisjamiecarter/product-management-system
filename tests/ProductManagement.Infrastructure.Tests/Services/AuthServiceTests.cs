using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NSubstitute;
using ProductManagement.Application.Constants;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Extensions;
using ProductManagement.Infrastructure.Interfaces;
using ProductManagement.Infrastructure.Models;
using ProductManagement.Infrastructure.Services;
using static ProductManagement.Application.Errors.ApplicationErrors;

namespace ProductManagement.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for the <see cref="AuthService"/> class, ensuring correct behavior.
/// </summary>
public class AuthServiceTests
{
    private static readonly ApplicationUser? DefaultNullApplicationUser = null;
    private static readonly Result DefaultIdentityFailureResult = IdentityResult.Failed(new IdentityError { Code = "Code", Description = "Description" }).ToDomainResult();
    private static readonly Result DefaultSuccessResult = Result.Success();

    private readonly IOptions<IdentityOptions> _options;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserManagerWrapper _userManagerWrapper;
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        _options = Substitute.For<IOptions<IdentityOptions>>();
        _userManager = Substitute.For<UserManager<ApplicationUser>>(Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _userManagerWrapper = Substitute.For<IUserManagerWrapper>();
        _signInManager = Substitute.For<SignInManager<ApplicationUser>>(_userManager, Substitute.For<IHttpContextAccessor>(), Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

        _authService = new AuthService(_options, _signInManager, _userManager, _userManagerWrapper);
    }

    [Fact]
    public async Task AddExternalLoginAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string provider = "provider";
        string providerKey = "providerKey";
        string providerDisplayName = "providerDisplayName";

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.AddExternalLoginAsync(userId, provider, providerKey, providerDisplayName);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task AddExternalLoginAsync_ReturnsFailure_WhenExternalLoginNotAdded()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, Email = email, UserName = email, EmailConfirmed = true };
        string provider = "provider";
        string providerKey = "providerKey";
        string providerDisplayName = "providerDisplayName";
        UserLoginInfo userLoginInfo = new UserLoginInfo(provider, providerKey, providerDisplayName);

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.FindByLoginAsync(provider, providerKey).Returns(DefaultNullApplicationUser);
        _userManagerWrapper.AddLoginAndReturnDomainResultAsync(user, userLoginInfo).ReturnsForAnyArgs(DefaultIdentityFailureResult);

        // Act.
        var result = await _authService.AddExternalLoginAsync(userId, provider, providerKey, providerDisplayName);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(ExternalLogin.NotAdded(userId), result.Error);
    }

    [Fact]
    public async Task AddExternalLoginAsync_ReturnsSuccess_WhenUserAlreadyHasExternalLogin()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, Email = email, UserName = email, EmailConfirmed = true };
        string provider = "provider";
        string providerKey = "providerKey";
        string providerDisplayName = "providerDisplayName";

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.FindByLoginAsync(provider, providerKey).Returns(user);

        // Act.
        var result = await _authService.AddExternalLoginAsync(userId, provider, providerKey, providerDisplayName);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task AddExternalLoginAsync_ReturnsSuccess_WhenExternalLoginAdded()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, Email = email, UserName = email, EmailConfirmed = true };
        string provider = "provider";
        string providerKey = "providerKey";
        string providerDisplayName = "providerDisplayName";
        UserLoginInfo userLoginInfo = new UserLoginInfo(provider, providerKey, providerDisplayName);

        _userManager.FindByEmailAsync(email).Returns(user);
        _userManager.FindByLoginAsync(provider, providerKey).Returns(DefaultNullApplicationUser);
        _userManagerWrapper.AddLoginAndReturnDomainResultAsync(user, userLoginInfo).ReturnsForAnyArgs(DefaultSuccessResult);

        // Act.
        var result = await _authService.AddExternalLoginAsync(email, provider, providerKey, providerDisplayName);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ConfirmEmailAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.ConfirmEmailAsync(userId, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task ConfirmEmailAsync_ReturnsFailure_WhenEmailNotConfirmed()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.ConfirmEmailAndReturnDomainResultAsync(user, token.Value).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _authService.ConfirmEmailAsync(user.Id, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task ConfirmEmailAsync_ReturnsSuccess_WhenEmailConfirmed()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.ConfirmEmailAndReturnDomainResultAsync(user, token.Value).Returns(DefaultSuccessResult);

        // Act.
        var result = await _authService.ConfirmEmailAsync(user.Id, token);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ExternalLoginSignInAsync_ReturnsFailure_WhenUserLockedOut()
    {
        // Arrange.
        string email = "user@testing.com";
        string provider = "provider";
        string providerKey = "providerKey";

        _signInManager.ExternalLoginSignInAsync(provider, providerKey, false, true).Returns(SignInResult.LockedOut);

        // Act.
        var result = await _authService.ExternalLoginSignInAsync(email, provider, providerKey);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.LockedOut(email), result.Error);
    }

    [Fact]
    public async Task ExternalLoginSignInAsync_ReturnsFailure_WhenUserNotAllowed()
    {
        // Arrange.
        string email = "user@testing.com";
        string provider = "provider";
        string providerKey = "providerKey";

        _signInManager.ExternalLoginSignInAsync(provider, providerKey, false, true).Returns(SignInResult.NotAllowed);

        // Act.
        var result = await _authService.ExternalLoginSignInAsync(email, provider, providerKey);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotAllowed(email), result.Error);
    }

    [Fact]
    public async Task ExternalLoginSignInAsync_ReturnsFailure_WhenUserRequiresTwoFactor()
    {
        // Arrange.
        string email = "user@testing.com";
        string provider = "provider";
        string providerKey = "providerKey";

        _signInManager.ExternalLoginSignInAsync(provider, providerKey, false, true).Returns(SignInResult.TwoFactorRequired);

        // Act.
        var result = await _authService.ExternalLoginSignInAsync(email, provider, providerKey);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.RequiresTwoFactor(email), result.Error);
    }

    [Fact]
    public async Task ExternalLoginSignInAsync_ReturnsFailure_WhenUserInvalidSignInAttempt()
    {
        // Arrange.
        string email = "user@testing.com";
        string provider = "provider";
        string providerKey = "providerKey";

        _signInManager.ExternalLoginSignInAsync(provider, providerKey, false, true).Returns(SignInResult.Failed);

        // Act.
        var result = await _authService.ExternalLoginSignInAsync(email, provider, providerKey);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.InvalidSignInAttempt(email), result.Error);
    }

    [Fact]
    public async Task ExternalLoginSignInAsync_ReturnsSuccess_WhenUserSignedIn()
    {
        // Arrange.
        string email = "user@testing.com";
        string provider = "provider";
        string providerKey = "providerKey";

        _signInManager.ExternalLoginSignInAsync(provider, providerKey, false, true).Returns(SignInResult.Success);

        // Act.
        var result = await _authService.ExternalLoginSignInAsync(email, provider, providerKey);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GenerateEmailChangeTokenAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string updatedEmail = "user@testing.com";

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.GenerateEmailChangeTokenAsync(userId, updatedEmail);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task GenerateEmailChangeTokenAsync_ReturnsSuccess_WhenTokenGenerated()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        string updatedEmail = "user@testing.com";

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GenerateChangeEmailTokenAsync(user, updatedEmail).Returns("token");

        // Act.
        var result = await _authService.GenerateEmailChangeTokenAsync(userId, updatedEmail);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GenerateEmailConfirmationTokenAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string email = "user@testing.com";

        _userManager.FindByEmailAsync(email).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.GenerateEmailConfirmationTokenAsync(email);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.EmailNotFound(email), result.Error);
    }

    [Fact]
    public async Task GenerateEmailConfirmationTokenAsync_ReturnsSuccess_WhenTokenGenerated()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, Email = email, UserName = email };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GenerateEmailConfirmationTokenAsync(user).Returns("token");

        // Act.
        var result = await _authService.GenerateEmailConfirmationTokenAsync(email);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GeneratePasswordResetTokenAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string email = "user@testing.com";

        _userManager.FindByEmailAsync(email).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.GeneratePasswordResetTokenAsync(email);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.EmailNotFound(email), result.Error);
    }

    [Fact]
    public async Task GeneratePasswordResetTokenAsync_ReturnsSuccess_WhenTokenGenerated()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, Email = email, UserName = email };

        _userManager.FindByEmailAsync(email).Returns(user);
        _userManager.GeneratePasswordResetTokenAsync(user).Returns("token");

        // Act.
        var result = await _authService.GeneratePasswordResetTokenAsync(email);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.GetCurrentUserAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ReturnsSuccess_WhenUserFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GetRolesAsync(user).Returns([Roles.User]);

        // Act.
        var result = await _authService.GetCurrentUserAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task GetExternalLoginInfo_ReturnsFailure_WhenExternalLoginInfoNotFound()
    {
        // Arrange.
        ExternalLoginInfo? info = null;

        _signInManager.GetExternalLoginInfoAsync().Returns(info);

        // Act.
        var result = await _authService.GetExternalLoginInfo();

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(ExternalLogin.NotFound, result.Error);
    }

    [Fact]
    public async Task GetExternalLoginInfo_ReturnsFailure_WhenEmailClaimIsNull()
    {
        // Arrange.
        string provider = "provider";
        string providerKey = "providerKey";
        string displayName = "displayName";
        ExternalLoginInfo? info = new ExternalLoginInfo(new ClaimsPrincipal(), provider, providerKey, displayName);

        //info.Principal.FindFirstValue(ClaimTypes.Email).Returns()
        _signInManager.GetExternalLoginInfoAsync().Returns(info);

        // Act.
        var result = await _authService.GetExternalLoginInfo();

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(ExternalLogin.NullEmailClaim, result.Error);
    }

    [Fact]
    public async Task PasswordSignInAsync_ReturnsFailure_WhenUserLockedOut()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        bool isPersistant = false;
        bool lockoutOnFailure = false;

        _signInManager.PasswordSignInAsync(email, password, isPersistant, lockoutOnFailure).Returns(SignInResult.LockedOut);

        // Act.
        var result = await _authService.PasswordSignInAsync(email, password, isPersistant);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.LockedOut(email), result.Error);
    }

    [Fact]
    public async Task PasswordSignInAsync_ReturnsFailure_WhenUserNotAllowed()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        bool isPersistant = false;
        bool lockoutOnFailure = false;

        _signInManager.PasswordSignInAsync(email, password, isPersistant, lockoutOnFailure).Returns(SignInResult.NotAllowed);

        // Act.
        var result = await _authService.PasswordSignInAsync(email, password, isPersistant);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotAllowed(email), result.Error);
    }

    [Fact]
    public async Task PasswordSignInAsync_ReturnsFailure_WhenUserRequiresTwoFactor()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        bool isPersistant = false;
        bool lockoutOnFailure = false;

        _signInManager.PasswordSignInAsync(email, password, isPersistant, lockoutOnFailure).Returns(SignInResult.TwoFactorRequired);

        // Act.
        var result = await _authService.PasswordSignInAsync(email, password, isPersistant);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.RequiresTwoFactor(email), result.Error);
    }

    [Fact]
    public async Task PasswordSignInAsync_ReturnsFailure_WhenUserInvalidSignInAttempt()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        bool isPersistant = false;
        bool lockoutOnFailure = false;

        _signInManager.PasswordSignInAsync(email, password, isPersistant, lockoutOnFailure).Returns(SignInResult.Failed);

        // Act.
        var result = await _authService.PasswordSignInAsync(email, password, isPersistant);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.InvalidSignInAttempt(email), result.Error);
    }

    [Fact]
    public async Task PasswordSignInAsync_ReturnsSuccess_WhenUserSignedIn()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        bool isPersistant = false;
        bool lockoutOnFailure = false;

        _signInManager.PasswordSignInAsync(email, password, isPersistant, lockoutOnFailure).Returns(SignInResult.Success);

        // Act.
        var result = await _authService.PasswordSignInAsync(email, password, isPersistant);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RefreshSignInAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.RefreshSignInAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task RefreshSignInAsync_ReturnsSuccess_WhenUserFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);

        // Act.
        var result = await _authService.RefreshSignInAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        ApplicationUser user = new ApplicationUser { Email = email, UserName = email };

        _userManagerWrapper.CreateAndReturnDomainResultAsync(user, password).ReturnsForAnyArgs(DefaultIdentityFailureResult);

        // Act.
        var result = await _authService.RegisterAsync(email, password);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsSuccess_WhenRegistered()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        ApplicationUser user = new ApplicationUser { Email = email, UserName = email };

        _userManagerWrapper.CreateAndReturnDomainResultAsync(user, password).ReturnsForAnyArgs(DefaultSuccessResult);

        // Act.
        var result = await _authService.RegisterAsync(email, password);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ResetPasswordAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        ApplicationUser user = new ApplicationUser { Email = email, UserName = email };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByEmailAsync(email).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.ResetPasswordAsync(email, password, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.EmailNotFound(email), result.Error);
    }

    [Fact]
    public async Task ResetPasswordAsync_ReturnsFailure_WhenPasswordNotReset()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        ApplicationUser user = new ApplicationUser { Email = email, UserName = email };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByEmailAsync(email).Returns(user);
        _userManagerWrapper.ResetPasswordAndReturnDomainResultAsync(user, token.Value, password).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _authService.ResetPasswordAsync(email, password, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task ResetPasswordAsync_ReturnsSuccess_WhenRegistered()
    {
        // Arrange.
        string email = "user@testing.com";
        string password = "password";
        ApplicationUser user = new ApplicationUser { Email = email, UserName = email };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByEmailAsync(email).Returns(user);
        _userManagerWrapper.ResetPasswordAndReturnDomainResultAsync(user, token.Value, password).Returns(DefaultSuccessResult);

        // Act.
        var result = await _authService.ResetPasswordAsync(email, password, token);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task SignInAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _authService.SignInAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task SignInAsync_ReturnsSuccess_WhenUserSignedIn()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);

        // Act.
        var result = await _authService.SignInAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task SignOutAsync_ReturnsSuccess()
    {
        // Arrange.

        // Act.
        var result = await _authService.SignOutAsync();

        // Assert.
        Assert.True(result.IsSuccess);
    }
}
