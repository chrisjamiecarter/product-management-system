using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Infrastructure.Constants;
using ProductManagement.Infrastructure.Contexts;
using ProductManagement.Infrastructure.EmailRender.Interfaces;
using ProductManagement.Infrastructure.EmailRender.Services;
using ProductManagement.Infrastructure.Interfaces;
using ProductManagement.Infrastructure.Models;
using ProductManagement.Infrastructure.Options;
using ProductManagement.Infrastructure.Repositories;
using ProductManagement.Infrastructure.Services;
using ProductManagement.Infrastructure.Wrappers;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace ProductManagement.Infrastructure.Installers;

/// <summary>
/// Registers dependencies and middleware for the Infrastructure layer.
/// </summary>
public static class InfrastructureInstaller
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductManagement") ?? throw new InvalidOperationException("Connection string 'ProductManagement' not found.");
        services.AddDbContext<ProductManagementDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsHistoryTable(Tables.EntityFrameworkCoreMigrations, Schemas.EntityFrameworkCore);
            });
        });

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = Application.Constants.PasswordOptions.RequiredLength;
            options.Password.RequireDigit = Application.Constants.PasswordOptions.RequireDigit;
            options.Password.RequireLowercase = Application.Constants.PasswordOptions.RequireLowercase;
            options.Password.RequireUppercase = Application.Constants.PasswordOptions.RequireUppercase;
            options.Password.RequireNonAlphanumeric = Application.Constants.PasswordOptions.RequireNonAlphanumeric;
            options.SignIn.RequireConfirmedAccount = Application.Constants.SignInOptions.RequireConfirmedAccount;
            options.User.RequireUniqueEmail = Application.Constants.UserOptions.RequireUniqueEmail;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ProductManagementDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

        var sqlLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    SchemaName = Schemas.Audit,
                    TableName = Tables.AuditLog
                },
                restrictedToMinimumLevel: LogEventLevel.Warning)
            .CreateLogger();
        
        services.AddLogging(builder =>
        {
            builder.AddSerilog(sqlLogger);
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.Configure<SeederOptions>(configuration.GetSection(nameof(SeederOptions)));
        services.AddScoped<SeederService>();

        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();
        services.AddScoped<IUserService, UserService>();

        services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        services.AddRazorPages();
        services.AddScoped<IRazorViewToStringRenderService, RazorViewToStringRenderService>();

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
        var seeder = serviceProvider.GetRequiredService<SeederService>();
        await seeder.SeedDatabaseAsync();

        return serviceProvider;
    }
}
