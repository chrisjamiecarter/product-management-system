using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Database.Contexts;
using ProductManagement.Infrastructure.Database.Identity;
using ProductManagement.Infrastructure.Database.Models;
using ProductManagement.Infrastructure.Email.Options;

namespace ProductManagement.Infrastructure.Database.Services;

internal class SeederService : ISeederService
{
    private static readonly string[] Roles = ["Owner", "Admin", "User"];

    private static readonly int Seed = 19890309;

    private readonly ProductManagementDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _userEmailStore;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IRoleStore<IdentityRole> _roleStore;
    private readonly SeederOptions _seederOptions;

    public SeederService(ProductManagementDbContext dbContext,
                         UserManager<ApplicationUser> userManager,
                         IUserStore<ApplicationUser> userStore,
                         RoleManager<IdentityRole> roleManager,
                         IRoleStore<IdentityRole> roleStore,
                         IOptions<SeederOptions> seederOptions)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _userStore = userStore;
        _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        _roleManager = roleManager;
        _roleStore = roleStore;
        _seederOptions = seederOptions.Value;
    }

    public async Task SeedDatabaseAsync()
    {
        if (!_seederOptions.SeedDatabase)
        {
            return;
        }

        await SeedRolesAsync(Roles);

        await SeedUserAsync(_seederOptions.Owner, "Owner");
        await SeedUserAsync(_seederOptions.Admin, "Admin");
        await SeedUserAsync(_seederOptions.User, "User");

        await SeedProductsAsync();
    }

    private async Task SeedRolesAsync(IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var identityRole = Activator.CreateInstance<IdentityRole>();
                await _roleStore.SetRoleNameAsync(identityRole, role, CancellationToken.None);

                await _roleManager.CreateAsync(identityRole);
            }
        }
    }

    private async Task SeedUserAsync(SeedUser user, string role)
    {
        if (await _userManager.FindByEmailAsync(user.Username) is null)
        {
            var applicationUser = Activator.CreateInstance<ApplicationUser>();
            await _userStore.SetUserNameAsync(applicationUser, user.Username, CancellationToken.None);
            await _userEmailStore.SetEmailAsync(applicationUser, user.Username, CancellationToken.None);
            await _userEmailStore.SetEmailConfirmedAsync(applicationUser, true, CancellationToken.None);
            
            var result = await _userManager.CreateAsync(applicationUser, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, role);
            }
        }
    }

    private async Task SeedProductsAsync()
    {
        if (_dbContext.Products.Any())
        {
            return;
        }

        var fakeProducts = new Faker<Product>()
            .UseSeed(Seed)
            .CustomInstantiator(f =>
            {
                return new Product(
                    f.Random.Guid(),
                    ProductName.Create(f.Commerce.ProductName()).Value,
                    f.Commerce.ProductDescription(),
                    ProductPrice.Create(decimal.Parse(f.Commerce.Price())).Value);
            });

        foreach (var fakeProduct in fakeProducts.Generate(_seederOptions.NumberOfProducts))
        {
            _dbContext.Products.Add(fakeProduct);
        }

        await _dbContext.SaveChangesAsync();
    }
}
