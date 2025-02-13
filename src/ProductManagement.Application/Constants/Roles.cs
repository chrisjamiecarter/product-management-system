namespace ProductManagement.Application.Constants;

/// <summary>
/// Defines the application roles.
/// </summary>
public static class Roles
{
    public static readonly string Owner = nameof(Owner);
    public static readonly string Admin = nameof(Admin);
    public static readonly string User = nameof(User);
    public static readonly string[] AllRoles = [Owner, Admin, User];
    public static readonly string[] ProductRoles = [Owner, User];
    public static readonly string[] UserRoles = [Owner, Admin];
}
