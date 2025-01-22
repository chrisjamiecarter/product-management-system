using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Models;
using ProductManagement.Application.Repositories;
using ProductManagement.Infrastructure.Database.Identity;

namespace ProductManagement.Infrastructure.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _userEmailStore;

    public UserRepository(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
    {
        _userManager = userManager;
        _userStore = userStore;
        _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
    }

    public async Task<bool> CreateAsync(string username, CancellationToken cancellationToken = default)
    {
        var applicationUser = Activator.CreateInstance<ApplicationUser>();
        await _userStore.SetUserNameAsync(applicationUser, username, cancellationToken);
        await _userEmailStore.SetEmailAsync(applicationUser, username, cancellationToken);
        var created = await _userManager.CreateAsync(applicationUser);
        return created.Succeeded;
    }

    public async Task<bool> DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByIdAsync(user.Id);
        if (applicationUser is null)
        {
            return false;
        }

        var deleted = await _userManager.DeleteAsync(applicationUser);
        return deleted.Succeeded;
    }

    public async Task<User?> ReturnByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByIdAsync(id);
        if (applicationUser is null)
        {
            return null;
        }

        return new User(applicationUser.Id, applicationUser.UserName, applicationUser.EmailConfirmed);
    }

    public async Task<PaginatedList<User>> ReturnByPageAsync(string? searchUsername, bool? searchEmailConfirmed, string? sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users;

        if (!string.IsNullOrWhiteSpace(searchUsername))
        {
            query = query.Where(u => u.UserName != null && u.UserName.Contains(searchUsername));
        }

        if (searchEmailConfirmed != null)
        {
            query = query.Where(u => u.EmailConfirmed == searchEmailConfirmed);
        }

        if (sortOrder?.ToLower() == "desc")
        {
            query = query.OrderByDescending(u => u.UserName);
        }
        else
        {
            query = query.OrderBy(u => u.UserName);
        }

        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        var users = items.Select(u => new User(u.Id, u.UserName, u.EmailConfirmed));

        return PaginatedList<User>.Create(users, count, pageNumber, pageSize);
    }

    public async Task<User?> ReturnByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByNameAsync(username);
        if (applicationUser is null)
        {
            return null;
        }

        return new User(applicationUser.Id, applicationUser.UserName, applicationUser.EmailConfirmed);
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByIdAsync(user.Id);
        if (applicationUser is null)
        {
            return false;
        }

        await _userStore.SetUserNameAsync(applicationUser, user.Username, cancellationToken);
        await _userEmailStore.SetEmailAsync(applicationUser, user.Username, cancellationToken);
        await _userEmailStore.SetEmailConfirmedAsync(applicationUser, user.EmailConfirmed, cancellationToken);

        var updated = await _userManager.UpdateAsync(applicationUser);
        return updated.Succeeded;
    }
}
