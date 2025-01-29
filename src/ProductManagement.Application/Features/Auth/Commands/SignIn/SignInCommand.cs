using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.SignIn;

public sealed record SignInCommand(string UserId) : ICommand;
