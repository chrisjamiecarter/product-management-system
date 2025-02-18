using Microsoft.AspNetCore.Identity;
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
/// Unit tests for the <see cref="UserService"/> class, ensuring correct behavior.
/// </summary>
public class UserServiceTests
{
    private static readonly ApplicationUser? DefaultNullApplicationUser = null;
    private static readonly Result DefaultIdentityFailureResult = IdentityResult.Failed(new IdentityError { Code = "Code", Description = "Description" }).ToDomainResult();
    private static readonly Result DefaultSuccessResult = Result.Success();

    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserManagerWrapper _userManagerWrapper;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(Substitute.For<IRoleStore<IdentityRole>>(), null, null, null, null);
        _userManager = Substitute.For<UserManager<ApplicationUser>>(Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _userManagerWrapper = Substitute.For<IUserManagerWrapper>();

        _userService = new UserService(_roleManager, _userManager, _userManagerWrapper);
    }

    [Fact]
    public async Task AddPasswordAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string password = "password";

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.AddPasswordAsync(userId, password);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task AddPasswordAsync_ReturnsFailure_WhenPasswordNotAdded()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string password = "password";
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.AddPasswordAndReturnDomainResultAsync(user, password).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.AddPasswordAsync(user.Id, password);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task AddPasswordAsync_ReturnsSuccess_WhenPasswordAdded()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string password = "password";
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.AddPasswordAndReturnDomainResultAsync(user, password).Returns(DefaultSuccessResult);

        // Act.
        var result = await _userService.AddPasswordAsync(user.Id, password);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        var email = "user@testing.com";
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.ChangeEmailAsync(userId, email, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsFailure_WhenEmailNotChanged()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        var email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, UserName = email, Email = email };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.ChangeEmailAndReturnDomainResultAsync(user, email, token.Value).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.ChangeEmailAsync(user.Id, email, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsFailure_WhenUsernameNotChanged()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        var email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, UserName = email, Email = email };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.ChangeEmailAndReturnDomainResultAsync(user, email, token.Value).Returns(DefaultSuccessResult);
        _userManagerWrapper.SetUserNameAndReturnDomainResultAsync(user, email).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.ChangeEmailAsync(user.Id, email, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsSuccess_WhenEmailChanged()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        var email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Id = userId, UserName = email, Email = email };
        AuthToken token = AuthToken.Encode("token");

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.ChangeEmailAndReturnDomainResultAsync(user, email, token.Value).Returns(DefaultSuccessResult);
        _userManagerWrapper.SetUserNameAndReturnDomainResultAsync(user, email).Returns(DefaultSuccessResult);

        // Act.
        var result = await _userService.ChangeEmailAsync(user.Id, email, token);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ChangePasswordAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string currentPassword = "currentPassword";
        string updatedPassword = "updatedPassword";

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.ChangePasswordAsync(userId, currentPassword, updatedPassword);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task ChangePasswordAsync_ReturnsFailure_WhenPasswordNotChanged()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string currentPassword = "currentPassword";
        string updatedPassword = "updatedPassword";
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);
        _userManagerWrapper.ChangePasswordAndReturnDomainResultAsync(user, currentPassword, updatedPassword).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.ChangePasswordAsync(userId, currentPassword, updatedPassword);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task ChangePasswordAsync_ReturnsSuccess_WhenPasswordChanged()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        string currentPassword = "currentPassword";
        string updatedPassword = "updatedPassword";
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.ChangePasswordAndReturnDomainResultAsync(user, currentPassword, updatedPassword).Returns(DefaultSuccessResult);

        // Act.
        var result = await _userService.ChangePasswordAsync(userId, currentPassword, updatedPassword);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CreateAsync_ReturnsFailure_WhenUserNotCreated()
    {
        // Arrange.
        var email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Email = email, UserName = email };

        _userManagerWrapper.CreateAndReturnDomainResultAsync(user).ReturnsForAnyArgs(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.CreateAsync(email, false);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task CreateAsync_ReturnsSuccess_WhenUserCreated()
    {
        // Arrange.
        var email = "user@testing.com";
        ApplicationUser user = new ApplicationUser { Email = email, UserName = email };

        _userManagerWrapper.CreateAndReturnDomainResultAsync(user).ReturnsForAnyArgs(DefaultSuccessResult);

        // Act.
        var result = await _userService.CreateAsync(email, false);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.DeleteAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFailure_WhenUserNotDeleted()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.DeleteAndReturnDomainResultAsync(user).ReturnsForAnyArgs(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.DeleteAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsSuccess_WhenUserDeleted()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManagerWrapper.DeleteAndReturnDomainResultAsync(user).ReturnsForAnyArgs(DefaultSuccessResult);

        // Act.
        var result = await _userService.DeleteAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task FindByEmailAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        var email = "user@testing.com";

        _userManager.FindByEmailAsync(email).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.FindByEmailAsync(email);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.EmailNotFound(email), result.Error);
    }

    [Fact]
    public async Task FindByEmailAsync_ReturnsSuccess_WhenUserFound()
    {
        // Arrange.
        var email = "user@testing.com";
        var user = new ApplicationUser { Email = email, UserName = email };

        _userManager.FindByEmailAsync(email).Returns(user);

        // Act.
        var result = await _userService.FindByEmailAsync(email);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(email, result.Value.Email);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.FindByIdAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsSuccess_WhenUserFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);

        // Act.
        var result = await _userService.FindByIdAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Value.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetPageAsync_ReturnsFailure_WhenInvalidPageNumber(int pageNumber)
    {
        // Arrange.
        int pageSize = 10;

        // Act.
        var result = await _userService.GetPageAsync(searchEmail: null,
                                                     searchEmailConfirmed: null,
                                                     searchRole: null,
                                                     sortColumn: null,
                                                     sortOrder: null,
                                                     pageNumber,
                                                     pageSize);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(PaginatedList.InvalidPageNumber(pageNumber), result.Error);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetPageAsync_ReturnsFailure_WhenInvalidPageSize(int pageSize)
    {
        // Arrange.
        int pageNumber = 1;

        // Act.
        var result = await _userService.GetPageAsync(searchEmail: null,
                                                     searchEmailConfirmed: null,
                                                     searchRole: null,
                                                     sortColumn: null,
                                                     sortOrder: null,
                                                     pageNumber,
                                                     pageSize);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(PaginatedList.InvalidPageSize(pageSize), result.Error);
    }

    [Fact]
    public async Task HasPasswordAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.HasPasswordAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task HasPasswordAsync_ReturnsSuccess_WhenUserFound(bool hasPassword)
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.HasPasswordAsync(user).Returns(hasPassword);

        // Act.
        var result = await _userService.HasPasswordAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(hasPassword, result.Value);
    }

    [Fact]
    public async Task IsEmailConfirmedAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.IsEmailConfirmedAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task IsEmailConfirmedAsync_ReturnsSuccess_WhenUserFound(bool emailConfirmed)
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.IsEmailConfirmedAsync(user).Returns(emailConfirmed);

        // Act.
        var result = await _userService.IsEmailConfirmedAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(emailConfirmed, result.Value);
    }

    [Fact]
    public async Task UpdateRoleAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        string updatedRole = "updatedRole";

        _userManager.FindByIdAsync(userId).Returns(DefaultNullApplicationUser);

        // Act.
        var result = await _userService.UpdateRoleAsync(userId, updatedRole);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(User.NotFound(userId), result.Error);
    }

    [Fact]
    public async Task UpdateRoleAsync_ReturnsFailure_WhenNotRemovedFromRole()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        string currentRole = "currentRole";
        string[] currentRoles = [currentRole];
        string updatedRole = "updatedRole";

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GetRolesAsync(user).Returns(currentRoles);
        _userManagerWrapper.RemoveFromRoleAndReturnDomainResultAsync(user, currentRole).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.UpdateRoleAsync(userId, updatedRole);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task UpdateRoleAsync_ReturnsFailure_WhenNotRemovedFromRoles()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        string[] currentRoles = ["currentRole1", "currentRole2", "currentRole3"];
        string updatedRole = "updatedRole";

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GetRolesAsync(user).Returns(currentRoles);
        _userManagerWrapper.RemoveFromRolesAndReturnDomainResultAsync(user, currentRoles).Returns(DefaultIdentityFailureResult);

        // Act.
        var result = await _userService.UpdateRoleAsync(userId, updatedRole);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(DefaultIdentityFailureResult.Error, result.Error);
    }

    [Fact]
    public async Task UpdateRoleAsync_ReturnsFailure_WhenRoleIsInvalid()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        string currentRole = "currentRole";
        string updatedRole = "updatedRole";

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GetRolesAsync(user).Returns([currentRole]);
        _userManagerWrapper.RemoveFromRoleAndReturnDomainResultAsync(user, currentRole).Returns(DefaultSuccessResult);

        // Act.
        var result = await _userService.UpdateRoleAsync(userId, updatedRole);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(Role.InvalidRole(updatedRole), result.Error);
    }

    [Fact]
    public async Task UpdateRoleAsync_ReturnsSuccess_WhenCurrentRoleIsEqualToUpdatedRole()
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        string currentRole = Roles.User;
        string updatedRole = Roles.User;

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GetRolesAsync(user).Returns([currentRole]);

        // Act.
        var result = await _userService.UpdateRoleAsync(userId, updatedRole);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Owner")]
    [InlineData("Admin")]
    [InlineData("User")]
    public async Task UpdateRoleAsync_ReturnsSuccess_WhenRoleUpdated(string updatedRole)
    {
        // Arrange.
        string userId = Guid.NewGuid().ToString();
        ApplicationUser user = new ApplicationUser { Id = userId };
        string[] currentRoles = ["lots", "of", "roles"];

        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.GetRolesAsync(user).Returns(x => currentRoles, x => []);
        _userManagerWrapper.RemoveFromRolesAndReturnDomainResultAsync(user, currentRoles).Returns(DefaultSuccessResult);
        _userManagerWrapper.RemoveFromRoleAndReturnDomainResultAsync(user, string.Empty).Returns(DefaultSuccessResult);
        _userManagerWrapper.AddToRoleAndReturnDomainResultAsync(user, updatedRole).Returns(DefaultSuccessResult);

        // Act.
        var result = await _userService.UpdateRoleAsync(userId, updatedRole);

        // Assert.
        Assert.True(result.IsSuccess);
    }
}
