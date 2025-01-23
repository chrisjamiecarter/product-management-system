using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Repositories;
using ProductManagement.Application.Services;
using ProductManagement.Infrastructure.Database.Contexts;
using ProductManagement.Infrastructure.Database.Identity;
using ProductManagement.Infrastructure.Database.Repositories;
using ProductManagement.Infrastructure.Database.Services;
using ProductManagement.Infrastructure.Email.Options;
using ProductManagement.Infrastructure.Email.Services;

namespace ProductManagement.Infrastructure.Installers;

/// <summary>
/// Registers dependencies and middleware for the Infrastructure layer.
/// </summary>
public static class InfrastructureInstaller
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductManagement") ?? throw new InvalidOperationException("Connection string 'ProductManagement' not found.");
        services.AddDbContext<ProductManagementDbContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ProductManagementDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.Configure<SeederOptions>(configuration.GetSection(nameof(SeederOptions)));
        services.AddScoped<ISeederService, SeederService>();

        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }

    public static async Task<IServiceProvider> MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ProductManagementDbContext>();
        await context.Database.MigrateAsync();

        return serviceProvider;
    }

    public static async Task<IServiceProvider> SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var seeder = serviceProvider.GetRequiredService<ISeederService>();
        await seeder.SeedDatabaseAsync();

        return serviceProvider;
    }
}
