using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Constants;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Extensions;
using ProductManagement.Infrastructure.Interfaces;
using ProductManagement.Infrastructure.Models;
using static ProductManagement.Application.Errors.ApplicationErrors;

namespace ProductManagement.Infrastructure.Services;

/// <summary>
/// Provides the service for user operations.
/// </summary>
internal class UserService : IUserService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserManagerWrapper _userManagerWrapper;

    public UserService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IUserManagerWrapper userManagerWrapper)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _userManagerWrapper = userManagerWrapper;
    }

    public async Task<Result> AddPasswordAsync(string userId, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        var result = await _userManagerWrapper.AddPasswordAndReturnDomainResultAsync(user, password);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result> ChangeEmailAsync(string userId, string updatedEmail, AuthToken token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        var emailResult = await _userManagerWrapper.ChangeEmailAndReturnDomainResultAsync(user, updatedEmail, token.Value);
        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        var usernameResult = await _userManagerWrapper.SetUserNameAndReturnDomainResultAsync(user, updatedEmail);
        if (usernameResult.IsFailure)
        {
            return Result.Failure(usernameResult.Error);
        }

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, string currentPassword, string updatedPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        var result = await _userManagerWrapper.ChangePasswordAndReturnDomainResultAsync(user, currentPassword, updatedPassword);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result> CreateAsync(string email, bool emailConfirmed, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
            EmailConfirmed = emailConfirmed,
        };

        var result = await _userManagerWrapper.CreateAndReturnDomainResultAsync(user);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        var result = await _userManagerWrapper.DeleteAndReturnDomainResultAsync(user);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result<ApplicationUserDto>> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(User.EmailNotFound(email));
        }

        var role = await GetUserRole(user);
        return Result.Success(user.ToDto(role));
    }

    public async Task<Result<ApplicationUserDto>> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(User.NotFound(userId));
        }

        var role = await GetUserRole(user);
        return Result.Success(user.ToDto(role));
    }

    public async Task<Result<ApplicationUserDto>> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
        if (user is null)
        {
            return Result.Failure<ApplicationUserDto>(User.LoginNotFound);
        }

        var role = await GetUserRole(user);
        return Result.Success(user.ToDto(role));
    }

    public async Task<Result<PaginatedList<ApplicationUserDto>>> GetPageAsync(string? searchEmail, bool? searchEmailConfirmed, string? searchRole, string? sortColumn, string? sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
        {
            return Result.Failure<PaginatedList<ApplicationUserDto>>(PaginatedList.InvalidPageNumber(pageNumber));
        }

        if (pageSize <= 0)
        {
            return Result.Failure<PaginatedList<ApplicationUserDto>>(PaginatedList.InvalidPageSize(pageSize));
        }

        var query = _userManager.Users.Select(u => new
        {
            User = u,
            RoleNames = _userManager.Users.Where(w => w.Id == u.Id)
            .SelectMany(s => s.UserRoles)
            .Join(_roleManager.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .ToList()
        });

        if (!string.IsNullOrWhiteSpace(searchEmail))
        {
            query = query.Where(q => q.User.Email != null && q.User.Email.Contains(searchEmail));
        }

        if (searchEmailConfirmed != null)
        {
            query = query.Where(q => q.User.EmailConfirmed == searchEmailConfirmed);
        }

        if (!string.IsNullOrWhiteSpace(searchRole))
        {
            query = query.Where(q => q.RoleNames.Contains(searchRole));
        }

        var isDesc = sortOrder?.ToLower() == "desc";
        query = sortColumn?.ToLower() switch
        {
            "emailconfirmed" when isDesc => query.OrderByDescending(q => q.User.EmailConfirmed).ThenBy(q => q.User.Email),
            "emailconfirmed" => query.OrderBy(q => q.User.EmailConfirmed).ThenBy(q => q.User.Email),
            "role" when isDesc => query.OrderByDescending(q => q.RoleNames.FirstOrDefault()).ThenBy(q => q.User.Email),
            "role" => query.OrderBy(q => q.RoleNames.FirstOrDefault()).ThenBy(q => q.User.Email),
            "email" when isDesc => query.OrderByDescending(q => q.User.Email),
            _ => query.OrderBy(q => q.User.Email),
        };

        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        var users = items.Select(item => new ApplicationUserDto(
            item.User.Id,
            item.User.Email,
            item.User.EmailConfirmed,
            item.RoleNames.FirstOrDefault() ?? null))
            .ToList();

        return Result.Success(PaginatedList<ApplicationUserDto>.Create(users, count, pageNumber, pageSize));
    }

    public async Task<Result<bool>> HasPasswordAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<bool>(User.NotFound(userId));
        }

        var response = await _userManager.HasPasswordAsync(user);
        return Result.Success(response);
    }

    public async Task<Result<bool>> IsEmailConfirmedAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<bool>(User.NotFound(userId));
        }

        var response = await _userManager.IsEmailConfirmedAsync(user);
        return Result.Success(response);
    }

    public async Task<Result> RemoveExternalLoginAsync(string userId, string provider, string providerKey, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        var result = await _userManagerWrapper.RemoveLoginAndReturnDomainResultAsync(user, provider, providerKey);
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    public async Task<Result> UpdateRoleAsync(string userId, string? role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(User.NotFound(userId));
        }

        // NOTE:
        // IdentityUser and IdentityRole has a Many to Many relationship by default.
        // In this app, we enforce user can only have one role.

        var currentRoles = await _userManager.GetRolesAsync(user);

        // Force removal if user has more than one role.
        if (currentRoles.Count > 1)
        {
            var forceRemoveRolesResult = await _userManagerWrapper.RemoveFromRolesAndReturnDomainResultAsync(user, currentRoles);
            if (forceRemoveRolesResult.IsFailure)
            {
                return Result.Failure(forceRemoveRolesResult.Error);
            }

            // Refresh.
            currentRoles = await _userManager.GetRolesAsync(user);
        }

        // Now there should only be 0 or 1 role.
        var currentRole = currentRoles.SingleOrDefault() ?? string.Empty;

        // Short-circuit if the user already has the exact role.
        if (currentRole == role)
        {
            return Result.Success();
        }

        // Otherwise remove.
        var removeRoleResult = await _userManagerWrapper.RemoveFromRoleAndReturnDomainResultAsync(user, currentRole);
        if (removeRoleResult.IsFailure)
        {
            return Result.Failure(removeRoleResult.Error);
        }

        // Then check is a valid role.
        if (!string.IsNullOrWhiteSpace(role) && !Roles.AllRoles.Contains(role))
        {
            return Result.Failure(Role.InvalidRole(role));
        }

        // Then add.
        var addRoleResult = await _userManagerWrapper.AddToRoleAndReturnDomainResultAsync(user, role);
        if (addRoleResult.IsFailure)
        {
            return Result.Failure(addRoleResult.Error);
        }

        return Result.Success();
    }

    private async Task<string?> GetUserRole(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user) ?? [];
        return roles.FirstOrDefault();
    }
}
