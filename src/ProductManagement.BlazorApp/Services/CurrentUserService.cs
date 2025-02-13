using System.Security.Claims;
using ProductManagement.Application.Interfaces.Application;

namespace ProductManagement.BlazorApp.Services;

/// <summary>
/// Provides the service for identifying the current authenticated user.
/// </summary>
internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
