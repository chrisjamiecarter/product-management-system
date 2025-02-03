using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(string UserId) : ICommand;
