using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.UpdateRole;

public sealed record UpdateRoleCommand(string UserId,
                                       string Role) : ICommand;
