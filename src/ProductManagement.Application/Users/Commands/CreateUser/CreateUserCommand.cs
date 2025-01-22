using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Username) : ICommand;
