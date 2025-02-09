namespace ProductManagement.Application.Features.Users.Queries.HasPassword;

/// <summary>
/// Provides extension methods for mapping <see cref="HasPasswordQuery"/> responses.
/// </summary>
internal static class HasPasswordMappingExtensions
{
    public static HasPasswordQueryResponse ToResponse(this bool hasPassword)
    {
        return new HasPasswordQueryResponse(hasPassword);
    }
}
