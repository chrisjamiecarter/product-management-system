using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailChange;

/// <summary>
/// Represents a command to generate an email change request for a user.
/// </summary>
public sealed record GenerateEmailChangeCommand(string UserId,
                                                string UpdatedEmail) : ICommand;
