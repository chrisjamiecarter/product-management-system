using Bogus;
using Microsoft.AspNetCore.Identity;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Database.Contexts;
using ProductManagement.Infrastructure.Database.Identity;

namespace ProductManagement.Infrastructure.Database.Services;

internal class SeederService : ISeederService
{
    private static readonly int Seed = 19890309;

    private readonly ProductManagementDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _userEmailStore;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IRoleStore<IdentityRole> _roleStore;

    public SeederService(ProductManagementDbContext dbContext,
                         UserManager<ApplicationUser> userManager,
                         IUserStore<ApplicationUser> userStore,
                         RoleManager<IdentityRole> roleManager,
                         IRoleStore<IdentityRole> roleStore)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _userStore = userStore;
        _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        _roleManager = roleManager;
        _roleStore = roleStore;
    }

    public async Task SeedDatabaseAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedProductsAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            var role = Activator.CreateInstance<IdentityRole>();
            await _roleStore.SetRoleNameAsync(role, "Admin", CancellationToken.None);
            var roleResult = await _roleManager.CreateAsync(role);
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            var role = Activator.CreateInstance<IdentityRole>();
            await _roleStore.SetRoleNameAsync(role, "User", CancellationToken.None);
            var roleResult = await _roleManager.CreateAsync(role);
        }
    }

    private async Task SeedUsersAsync()
    {
        var adminEmail = "admin@email.com";
        var adminPassword = "Admin123###";
        if (await _userManager.FindByEmailAsync(adminEmail) is null)
        {
            var user = Activator.CreateInstance<ApplicationUser>();
            await _userStore.SetUserNameAsync(user, adminEmail, CancellationToken.None);
            await _userEmailStore.SetEmailAsync(user, adminEmail, CancellationToken.None);
            await _userEmailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);
            var userResult = await _userManager.CreateAsync(user, adminPassword);
            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
        }

        var userEmail = "user@email.com";
        var userPassword = "User123###";
        if (await _userManager.FindByEmailAsync(userEmail) is null)
        {
            var user = Activator.CreateInstance<ApplicationUser>();
            await _userStore.SetUserNameAsync(user, userEmail, CancellationToken.None);
            await _userEmailStore.SetEmailAsync(user, userEmail, CancellationToken.None);
            await _userEmailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);
            var userResult = await _userManager.CreateAsync(user, userPassword);
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
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

        foreach (var fakeProduct in fakeProducts.Generate(100))
        {
            _dbContext.Products.Add(fakeProduct);
        }

        await _dbContext.SaveChangesAsync();
    }
}
