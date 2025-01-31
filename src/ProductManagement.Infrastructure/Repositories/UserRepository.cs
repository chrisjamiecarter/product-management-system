using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Repositories;

/// <summary>
/// TODO: Turn into a thin repository for UserMananger only.
/// </summary>
internal class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _userEmailStore;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _userStore = userStore;
        _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        _roleManager = roleManager;
    }

    public async Task<bool> CreateAsync(string username, string role, CancellationToken cancellationToken = default)
    {
        var applicationUser = Activator.CreateInstance<ApplicationUser>();
        await _userStore.SetUserNameAsync(applicationUser, username, cancellationToken);
        await _userEmailStore.SetEmailAsync(applicationUser, username, cancellationToken);

        var userCreated = await _userManager.CreateAsync(applicationUser);
        if (userCreated.Succeeded && !string.IsNullOrWhiteSpace(role))
        {
            var addedToRole = await _userManager.AddToRoleAsync(applicationUser, role);
            return addedToRole.Succeeded;
        }

        return userCreated.Succeeded;
    }

    public async Task<bool> DeleteAsync(ApplicationUserDto user, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByIdAsync(user.Id);
        if (applicationUser is null)
        {
            return false;
        }

        var deleted = await _userManager.DeleteAsync(applicationUser);
        return deleted.Succeeded;
    }

    public async Task<IReadOnlyList<ApplicationUserDto>> ReturnAllAsync(CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users.Select(u => new
        {
            User = u,
            RoleNames = _userManager.Users.Where(w => w.Id == u.Id)
            .SelectMany(s => s.UserRoles)
            .Join(_roleManager.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .ToList()
        });

        return await query.Select(item => new ApplicationUserDto(item.User.Id,
                                                                 item.User.UserName,
                                                                 item.RoleNames.FirstOrDefault() ?? null,
                                                                 item.User.EmailConfirmed))
                         .OrderBy(u => u.Username)
                         .ToListAsync(cancellationToken);
    }

    public async Task<ApplicationUserDto?> ReturnByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByEmailAsync(email);
        if (applicationUser is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(applicationUser);
        var role = roles.Any() ? roles.FirstOrDefault() : null;

        return new ApplicationUserDto(applicationUser.Id, applicationUser.UserName, role, applicationUser.EmailConfirmed);
    }

    public async Task<ApplicationUserDto?> ReturnByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByIdAsync(id);
        if (applicationUser is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(applicationUser);
        var role = roles.Any() ? roles.FirstOrDefault() : null;

        return new ApplicationUserDto(applicationUser.Id, applicationUser.UserName, role, applicationUser.EmailConfirmed);
    }

    public async Task<PaginatedList<ApplicationUserDto>> ReturnByPageAsync(string? searchUsername, string? searchRole, bool? searchEmailConfirmed, string? sortColumn, string? sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users.Select(u => new
        {
            User = u,
            RoleNames = _userManager.Users.Where(w => w.Id == u.Id)
            .SelectMany(s => s.UserRoles)
            .Join(_roleManager.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .ToList()
        });

        if (!string.IsNullOrWhiteSpace(searchUsername))
        {
            query = query.Where(q => q.User.UserName != null && q.User.UserName.Contains(searchUsername));
        }

        if (!string.IsNullOrWhiteSpace(searchRole))
        {
            query = query.Where(q => q.RoleNames.Contains(searchRole));
        }

        if (searchEmailConfirmed != null)
        {
            query = query.Where(q => q.User.EmailConfirmed == searchEmailConfirmed);
        }

        var isDesc = sortOrder?.ToLower() == "desc";
        query = sortColumn?.ToLower() switch
        {
            "emailconfirmed" when isDesc => query.OrderByDescending(q => q.User.EmailConfirmed).ThenBy(q => q.User.UserName),
            "emailconfirmed" => query.OrderBy(q => q.User.EmailConfirmed).ThenBy(q => q.User.UserName),
            "role" when isDesc => query.OrderByDescending(q => q.RoleNames.FirstOrDefault()).ThenBy(q => q.User.UserName),
            "role" => query.OrderBy(q => q.RoleNames.FirstOrDefault()).ThenBy(q => q.User.UserName),
            "username" when isDesc => query.OrderByDescending(q => q.User.UserName),
            _ => query.OrderBy(q => q.User.UserName),
        };

        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        var users = items.Select(item => new ApplicationUserDto(
            item.User.Id,
            item.User.UserName,
            item.RoleNames.FirstOrDefault() ?? null,
            item.User.EmailConfirmed))
            .ToList();

        return PaginatedList<ApplicationUserDto>.Create(users, count, pageNumber, pageSize);
    }

    public async Task<ApplicationUserDto?> ReturnByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByNameAsync(username);
        if (applicationUser is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(applicationUser);
        var role = roles.Any() ? roles.FirstOrDefault() : null;

        return new ApplicationUserDto(applicationUser.Id, applicationUser.UserName, role, applicationUser.EmailConfirmed);
    }

    public async Task<bool> UpdateAsync(ApplicationUserDto user, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByIdAsync(user.Id);
        if (applicationUser is null)
        {
            return false;
        }

        await _userStore.SetUserNameAsync(applicationUser, user.Username, cancellationToken);
        await _userEmailStore.SetEmailAsync(applicationUser, user.Username, cancellationToken);
        await _userEmailStore.SetEmailConfirmedAsync(applicationUser, user.EmailConfirmed, cancellationToken);

        var userUpdated = await _userManager.UpdateAsync(applicationUser);

        var roles = await _userManager.GetRolesAsync(applicationUser);
        if (roles.Count > 1)
        {
            foreach (var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(applicationUser, role);
            }
        }

        var existingRole = roles.Any() ? roles.FirstOrDefault() : null;
        if (existingRole != user.Role && user.HasRole)
        {
            await _userManager.AddToRoleAsync(applicationUser, user.Role!);
        }

        return userUpdated.Succeeded;
    }
}
