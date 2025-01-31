using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailConfirmation;

public sealed record GenerateEmailConfirmationCommand(string Email) : ICommand;
