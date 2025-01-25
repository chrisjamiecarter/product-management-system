using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Username,
                                       string Role) : ICommand;
