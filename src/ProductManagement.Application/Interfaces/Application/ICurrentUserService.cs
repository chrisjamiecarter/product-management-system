using System.Security.Claims;

namespace ProductManagement.Application.Interfaces.Application;

public interface ICurrentUserService
{
    string? UserId { get; }
}
