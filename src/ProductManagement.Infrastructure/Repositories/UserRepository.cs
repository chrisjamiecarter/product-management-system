using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Repositories;

/// <summary>
/// TODO: Turn into a thin repository for UserMananger only.
/// </summary>
internal class UserRepository
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
