using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.SignInUser;

public sealed record SignInUserCommand(string Email,
                                       string Password,
                                       bool Remember) : ICommand;
