namespace ProductManagement.Infrastructure.Constants;

/// <summary>
/// Defines the table names used by the database.
/// </summary>
internal static class Tables
{
    public static readonly string ApplicationUser = "Users";
    public static readonly string ApplicationUserClaim = "UserClaims";
    public static readonly string ApplicationUserLogin = "UserLogins";
    public static readonly string ApplicationUserRole = "UserRoles";
    public static readonly string ApplicationUserToken = "UserTokens";
    public static readonly string AuditLog = "Log";
    public static readonly string EntityFrameworkCoreMigrations = "MigrationsHistory";
    public static readonly string Product = "Product";
    public static readonly string Role = "Roles";
    public static readonly string RoleClaim = "RoleClaims";
}
