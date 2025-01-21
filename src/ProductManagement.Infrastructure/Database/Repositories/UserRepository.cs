using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Models;
using ProductManagement.Application.Repositories;
using ProductManagement.Infrastructure.Database.Identity;

namespace ProductManagement.Infrastructure.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<PaginatedList<User>> ReturnByPageAsync(string? searchUserName, bool? searchEmailConfirmed, string? sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users;

        if (!string.IsNullOrWhiteSpace(searchUserName))
        {
            query = query.Where(u => u.UserName != null && u.UserName.Contains(searchUserName));
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

        var users = items.Select(u => new User
        {
            Id = u.Id,
            UserName = u.UserName,
            EmailConfirmed = u.EmailConfirmed,
        });

        return PaginatedList<User>.Create(users, count, pageNumber, pageSize);
    }
}
