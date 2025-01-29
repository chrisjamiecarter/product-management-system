using Microsoft.AspNetCore.Identity;
using Moq;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Infrastructure.Database.Models;
using ProductManagement.Infrastructure.Errors;
using ProductManagement.Infrastructure.Services;

namespace ProductManagement.Infrastructure.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        _userService = new UserService(_userManagerMock.Object);
    }

    [Fact]
    public async Task AddPasswordAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        var user = new ApplicationUser { Id = "userId" };
        var password = "TestPassword123###";
        ApplicationUser? nullUser = null;

        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id))
                        .ReturnsAsync(nullUser);

        _userManagerMock.Setup(m => m.AddPasswordAsync(user, password))
                        .ReturnsAsync(IdentityResult.Success);

        // Act.
        var result = await _userService.AddPasswordAsync(user.Id, password);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task AddPasswordAsync_ReturnsFailure_WhenUserPasswordNotAdded()
    {
        // Arrange.
        var user = new ApplicationUser { Id = "userId" };
        var password = "TestPassword123###";

        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id))
                        .ReturnsAsync(user);

        _userManagerMock.Setup(m => m.AddPasswordAsync(user, password))
                        .ReturnsAsync(IdentityResult.Failed());

        // Act.
        var result = await _userService.AddPasswordAsync(user.Id, password);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.PasswordNotAdded, result.Error);
    }

    [Fact]
    public async Task AddPasswordAsync_ReturnsSuccess_WhenUserPasswordChanged()
    {
        // Arrange.
        var user = new ApplicationUser { Id = "userId" };
        var password = "TestPassword123###";

        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id))
                        .ReturnsAsync(user);

        _userManagerMock.Setup(m => m.AddPasswordAsync(user, password))
                        .ReturnsAsync(IdentityResult.Success);

        // Act.
        var result = await _userService.AddPasswordAsync(user.Id, password);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        var userId = "userId";
        var token = AuthToken.Encode("token");

        _userManagerMock.Setup(m => m.FindByIdAsync(userId))
                        .ReturnsAsync((ApplicationUser?)null);

        // Act.
        var result = await _userService.ChangeEmailAsync(userId, "email", token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsFailure_WhenUserEmailNotChanged()
    {
        // Arrange.
        var user = new ApplicationUser();
        var newEmail = "newEmail";
        var token = AuthToken.Encode("token");

        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id))
                        .ReturnsAsync(user);

        _userManagerMock.Setup(m => m.ChangeEmailAsync(user, newEmail, token.Value))
                        .ReturnsAsync(IdentityResult.Failed());

        // Act.
        var result = await _userService.ChangeEmailAsync(user.Id, newEmail, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.EmailNotChanged, result.Error);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsFailure_WhenUserUsernameNotChanged()
    {
        // Arrange.
        var user = new ApplicationUser();
        var newEmail = "newEmail";
        var token = AuthToken.Encode("token");

        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id))
                        .ReturnsAsync(user);

        _userManagerMock.Setup(m => m.ChangeEmailAsync(user, newEmail, token.Value))
                        .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(m => m.SetUserNameAsync(user, newEmail))
                        .ReturnsAsync(IdentityResult.Failed());

        // Act.
        var result = await _userService.ChangeEmailAsync(user.Id, newEmail, token);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.UsernameNotChanged, result.Error);
    }

    [Fact]
    public async Task ChangeEmailAsync_ReturnsSuccess_WhenUserEmailChanged()
    {
        // Arrange.
        var user = new ApplicationUser { Id = "userId" };
        var newEmail = "newEmail";
        var token = AuthToken.Encode("token");

        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id))
                        .ReturnsAsync(user);

        _userManagerMock.Setup(m => m.ChangeEmailAsync(user, newEmail, token.Value))
                        .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(m => m.SetUserNameAsync(user, newEmail))
                        .ReturnsAsync(IdentityResult.Success);

        // Act.
        var result = await _userService.ChangeEmailAsync(user.Id, newEmail, token);

        // Assert.
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task FindByEmailAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        var email = "user@example.com";
        ApplicationUser? nullUser = null;

        _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(nullUser);

        // Act.
        var result = await _userService.FindByEmailAsync(email);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task FindByEmailAsync_ReturnsSuccess_WhenUserFound()
    {
        // Arrange.
        var email = "user@example.com";
        var user = new ApplicationUser { Email = email, UserName = email };

        _userManagerMock.Setup(m => m.FindByEmailAsync(email))
                        .ReturnsAsync(user);

        // Act.
        var result = await _userService.FindByEmailAsync(email);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(email, result.Value.Username);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        var userId = "userId";
        ApplicationUser? nullUser = null;

        _userManagerMock.Setup(m => m.FindByIdAsync(userId))
                        .ReturnsAsync(nullUser);

        // Act.
        var result = await _userService.FindByIdAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsSuccess_WhenUserFound()
    {
        // Arrange.
        var userId = "userId";
        var user = new ApplicationUser { Id = userId };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId))
                        .ReturnsAsync(user);

        // Act.
        var result = await _userService.FindByIdAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Value.Id);
    }

    [Fact]
    public async Task HasPasswordAsync_ReturnsFailure_WhenUserNotFound()
    {
        // Arrange.
        var userId = "userId";
        ApplicationUser? nullUser = null;

        _userManagerMock.Setup(m => m.FindByIdAsync(userId))
                        .ReturnsAsync(nullUser);

        // Act.
        var result = await _userService.HasPasswordAsync(userId);

        // Assert.
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task HasPasswordAsync_ReturnsSuccessAndFalse_WhenUserFoundAndHasPasswordIsFalse()
    {
        // Arrange.
        var userId = "userId";
        var user = new ApplicationUser { Id = userId };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId))
                        .ReturnsAsync(user);

        _userManagerMock.Setup(m => m.HasPasswordAsync(user))
                        .ReturnsAsync(false);

        // Act.
        var result = await _userService.HasPasswordAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
    }

    [Fact]
    public async Task HasPasswordAsync_ReturnsSuccessAndTrue_WhenUserFoundAndHasPasswordIsTrue()
    {
        // Arrange.
        var userId = "userId";
        var user = new ApplicationUser { Id = userId };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId))
                        .ReturnsAsync(user);

        _userManagerMock.Setup(m => m.HasPasswordAsync(user))
                        .ReturnsAsync(true);

        // Act.
        var result = await _userService.HasPasswordAsync(userId);

        // Assert.
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }
}
