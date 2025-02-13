using System.Text;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Constants;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Contexts;
using ProductManagement.Infrastructure.Models;
using ProductManagement.Infrastructure.Options;

namespace ProductManagement.Infrastructure.Services;

/// <summary>
/// Provides the service for seeding the database.
/// </summary>
internal class SeederService
{
    private static readonly int Seed = 19890309;

    private readonly ProductManagementDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SeederOptions _seederOptions;

    public SeederService(ProductManagementDbContext dbContext,
                         UserManager<ApplicationUser> userManager,
                         RoleManager<IdentityRole> roleManager,
                         IOptions<SeederOptions> seederOptions)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _seederOptions = seederOptions.Value;
    }

    public async Task SeedDatabaseAsync()
    {
        if (!_seederOptions.SeedDatabase)
        {
            return;
        }

        foreach (var role in Roles.AllRoles)
        {
            await SeedRoleAsync(role);
        }

        foreach(var user in _seederOptions.SeedUsers)
        {
            await SeedUserAsync(user);
        }

        await SeedProductsAsync();
    }

    private async Task SeedRoleAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var identityRole = new IdentityRole(roleName);
            await _roleManager.CreateAsync(identityRole);
        }
    }

    private async Task SeedUserAsync(SeedUser user)
    {
        if (await _userManager.FindByEmailAsync(user.Email) is null)
        {
            var applicationUser = new ApplicationUser
            {
                Email = user.Email,
                EmailConfirmed = true,
                UserName = user.Email,
            };

            var p = new StringBuilder();
            var userName = user.Email[..user.Email.IndexOf('@')].ToLower();
            p.Append(char.ToUpper(userName[0]));
            p.Append(userName[1 ..]);
            p.Append("123###");
                        
            var result = await _userManager.CreateAsync(applicationUser, p.ToString());
            if (result.Succeeded)
            {
                await SeedRoleAsync(user.Role);
                await _userManager.AddToRoleAsync(applicationUser, user.Role);
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
