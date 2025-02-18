using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Constants;
using ProductManagement.Application.Interfaces.Application;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.BlazorApp.Abstractions.Messaging;
using ProductManagement.BlazorApp.Components;
using ProductManagement.BlazorApp.Components.Account;
using ProductManagement.BlazorApp.Interfaces;
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
        services.AddRazorComponents()
                .AddInteractiveServerComponents();

        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Signin";
        });

        services.AddAuthorizationBuilder()
                .AddPolicy(Policies.RequireProductRole, policy => policy.RequireRole(Roles.ProductRoles))
                .AddPolicy(Policies.RequireUserRole, policy => policy.RequireRole(Roles.UserRoles));

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ISenderService, SenderService>();
        services.AddScoped<IToastService, ToastService>();
        services.AddScoped<ILinkBuilderService, LinkBuilderService>();

        return services;
    }

    public static WebApplication AddMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/Error/{0}");

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapAdditionalIdentityEndpoints();

        return app;
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
