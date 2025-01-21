using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Installers;
using ProductManagement.BlazorApp.Components;
using ProductManagement.BlazorApp.Components.Account;
using ProductManagement.Infrastructure.Database.Identity;
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
        
        // TODO: Refactor the below.

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
            .AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));

        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        //builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseSqlServer(connectionString);
        //    //options.UseSeeding((context, _) =>
        //    //{
        //    //    var defaultAdminEmail = "admin@email.com";
        //    //    var defaultAdminUser = new ApplicationUser
        //    //    {
        //    //        UserName = defaultAdminEmail,
        //    //        NormalizedUserName = defaultAdminEmail.ToUpper(),
        //    //        Email = defaultAdminEmail,
        //    //        NormalizedEmail = defaultAdminEmail.ToUpper(),
        //    //        EmailConfirmed = true,
        //    //    };

        //    //    var contains = context.Set<ApplicationUser>().Any(x => x.UserName == defaultAdminUser.UserName);
        //    //    if (!contains)
        //    //    {
        //    //        defaultAdminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(defaultAdminUser, "Admin123###");
        //    //        context.Set<ApplicationUser>().Add(defaultAdminUser);
        //    //        context.SaveChanges();
        //    //    }

        //    //});
        //});

        //builder.Services.AddIdentityCore<ApplicationUser>(options =>
        //{
        //    options.Password.RequiredLength = 8;
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireLowercase = true;
        //    options.Password.RequireUppercase = true;
        //    options.Password.RequireNonAlphanumeric = true;
        //    options.SignIn.RequireConfirmedAccount = true;
        //})
        //.AddRoles<IdentityRole>()
        //.AddEntityFrameworkStores<ApplicationDbContext>()
        //.AddSignInManager()
        //.AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        var app = builder.Build();
        await app.SetUpDatabaseAsync();

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

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}
