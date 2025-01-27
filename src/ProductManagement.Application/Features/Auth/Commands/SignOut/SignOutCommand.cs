using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.SignOut;

public sealed record SignOutCommand() : ICommand;
