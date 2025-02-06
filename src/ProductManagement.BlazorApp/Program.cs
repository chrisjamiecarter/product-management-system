using ProductManagement.Application.Installers;
using ProductManagement.BlazorApp.Installers;
using ProductManagement.Infrastructure.Installers;

namespace ProductManagement.BlazorApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPresentation();

        var app = builder.Build();
        app.AddMiddleware();
        await app.SetUpDatabaseAsync();
        app.Run();
    }
}
