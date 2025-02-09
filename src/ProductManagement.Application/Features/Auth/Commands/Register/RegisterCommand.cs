using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.Register;

/// <summary>
/// Represents a command to register a new user.
/// </summary>
public sealed record RegisterCommand(string Email,
                                     string? Password,
                                     string? Role) : ICommand;
