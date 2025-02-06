namespace ProductManagement.Infrastructure.Constants;

internal static class Schemas
{
    public static readonly string Audit = "audit";
    public static readonly string Core = "core";
    // TODO: make migrations table point to this schema.
    public static readonly string EntityFrameworkCore = "efcore";
    public static readonly string Identity = "security";
}
