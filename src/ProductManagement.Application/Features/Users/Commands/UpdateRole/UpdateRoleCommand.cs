using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.UpdateRole;

/// <summary>
/// Represents a command to updated an existing users role.
/// </summary>
public sealed record UpdateRoleCommand(string UserId,
                                       string Role) : ICommand;
