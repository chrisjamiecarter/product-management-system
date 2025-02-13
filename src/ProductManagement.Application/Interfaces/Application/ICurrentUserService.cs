namespace ProductManagement.Application.Interfaces.Application;

/// <summary>
/// Defines the service for identifying the current authenticated user.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
}
