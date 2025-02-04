using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Errors;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Services;

internal class UserService : IUserService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<Result> AddPasswordAsync(string userId, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        var result = await _userManager.AddPasswordAsync(user, password);
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.PasswordNotAdded);
    }

    public async Task<Result> ChangeEmailAsync(string userId, string newEmail, AuthToken token, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        var emailResult = await _userManager.ChangeEmailAsync(user, newEmail, token.Value);
        if (!emailResult.Succeeded)
        {
            return Result.Failure(UserErrors.EmailNotChanged);
        }

        var usernameResult = await _userManager.SetUserNameAsync(user, newEmail);
        if (!usernameResult.Succeeded)
        {
            return Result.Failure(UserErrors.UsernameNotChanged);
        }

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
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

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        var deleted = await _userManager.DeleteAsync(user);
        return deleted.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.NotDeleted);
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

    public async Task<Result<PaginatedList<ApplicationUserDto>>> GetPageAsync(string? searchEmail, bool? searchEmailConfirmed, string? searchRole, string? sortColumn, string? sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        // TODO: these need to go back to app errors.
        if (pageNumber <= 0)
        {
            return Result.Failure<PaginatedList<ApplicationUserDto>>(ApplicationErrors.PaginatedList.InvalidPageNumber);
        }

        if (pageSize <= 0)
        {
            return Result.Failure<PaginatedList<ApplicationUserDto>>(ApplicationErrors.PaginatedList.InvalidPageSize);
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

        return PaginatedList<ApplicationUserDto>.Create(users, count, pageNumber, pageSize);
    }

    public async Task<Result<bool>> HasPasswordAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<bool>(UserErrors.NotFound);
        }

        var response = await _userManager.HasPasswordAsync(user);
        return Result.Success(response);
    }

    public async Task<Result<bool>> IsEmailConfirmedAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<bool>(UserErrors.NotFound);
        }

        var response = await _userManager.IsEmailConfirmedAsync(user);
        return Result.Success(response);
    }

    public async Task<Result> UpdateRoleAsync(string userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        // NOTE: User can have many Roles, but in this app we treat role as singular.
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.FirstOrDefault() != role)
        {
            foreach (var currentRole in  currentRoles)
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRole);
                if (!removeResult.Succeeded)
                {
                    return Result.Failure(UserErrors.NotAddedToRole);
                }
            }
        }

        // NOTE: Only add to valid role - Not whitespace.
        if (!string.IsNullOrWhiteSpace(role))
        {
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                return Result.Failure(UserErrors.NotAddedToRole);
            }
        }

        return Result.Success();
    }

    private async Task<string?> GetUserRole(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user) ?? [];
        return roles.FirstOrDefault();
    }
}
