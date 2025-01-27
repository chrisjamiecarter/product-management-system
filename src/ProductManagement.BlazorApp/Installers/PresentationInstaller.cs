using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Services;
using ProductManagement.BlazorApp.Abstractions.Messaging;
using ProductManagement.BlazorApp.Services;
using ProductManagement.Infrastructure.Installers;

namespace ProductManagement.BlazorApp.Installers;

/// <summary>
/// Registers dependencies and middleware for the Presentation layer.
/// </summary>
public static class PresentationInstaller
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<SenderService>();
        services.AddScoped<ToastService>();

        return services;
    }

    public static async Task<WebApplication> SetUpDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        await services.MigrateDatabaseAsync();
        await services.SeedDatabaseAsync();

        return app;
    }
}
