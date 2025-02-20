using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.RemoveExternalLogin;

/// <summary>
/// Represents a command to remove an external login from an existing user.
/// </summary>
public sealed record RemoveExternalLoginCommand(string UserId,
                                                string Provider,
                                                string ProviderKey) : ICommand;
