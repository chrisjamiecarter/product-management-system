using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.PasswordSignIn;

public sealed record PasswordSignInCommand(string Email,
                                           string Password,
                                           bool Remember) : ICommand;
