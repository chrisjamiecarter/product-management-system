using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Models;
using ProductManagement.Infrastructure.Errors;

namespace ProductManagement.Infrastructure.Services;

internal class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> ChangeEmailAsync(string userId, string email, string token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        var emailResult = await _userManager.ChangeEmailAsync(user, email, token);
        if (!emailResult.Succeeded)
        {
            return Result.Failure(UserErrors.EmailNotChanged);
        }

        var usernameResult = await _userManager.SetUserNameAsync(user, email);
        if (!usernameResult.Succeeded)
        {
            return Result.Failure(UserErrors.UsernameNotChanged);
        }

        return Result.Success();
    }

    public async Task<Result<ApplicationUserDto>> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(UserErrors.NotFound);
        }

        var role = await GetUserRole(user);
        return Result.Success(user.ToDto(role));
    }
    
    public async Task<Result<ApplicationUserDto>> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(UserErrors.NotFound);
        }

        var role = await GetUserRole(user);
        return Result.Success(user.ToDto(role));
    }

    private async Task<string?> GetUserRole(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user) ?? [];
        return roles.FirstOrDefault();
    }
}
