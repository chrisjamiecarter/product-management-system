using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Database.Contexts;
using ProductManagement.Infrastructure.Database.Repositories;

namespace ProductManagement.Infrastructure.Installers;

/// <summary>
/// Registers dependencies and middleware for the Infrastructure layer.
/// </summary>
public static class InfrastructureInstaller
{
    private static readonly int Seed = 19890309;

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductManagement") ?? throw new InvalidOperationException("Connection string 'ProductManagement' not found.");
        services.AddDbContext<ProductManagementDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseSeeding((context, _) =>
            {
                SeedDatabase(context);
            });
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    public static IServiceProvider MigrateDatabase(this IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<ProductManagementDbContext>();
        context.Database.Migrate();

        return serviceProvider;
    }

    private static void SeedDatabase(DbContext context)
    {
        var products = context.Set<Product>();
        if (products.Any())
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
            products.Add(fakeProduct);
        }

        context.SaveChanges();
    }
}
