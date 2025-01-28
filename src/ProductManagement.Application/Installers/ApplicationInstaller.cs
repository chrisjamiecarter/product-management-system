using Microsoft.Extensions.DependencyInjection;

namespace ProductManagement.Application.Installers;

/// <summary>
/// Registers dependencies and middleware for the Application layer.
/// </summary>
public static class ApplicationInstaller
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        return services;
    }
}
