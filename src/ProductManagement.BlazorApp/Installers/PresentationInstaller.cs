using ProductManagement.Application.Services;
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
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<IToastService, ToastService>();

        return services;
    }

    public static WebApplication SetUpDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        services.MigrateDatabase();

        return app;
    }
}
