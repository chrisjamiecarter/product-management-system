using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.LinkExternalLogin;

/// <summary>
/// Represents a command to link an external login to an existing user.
/// </summary>
public sealed record LinkExternalLoginCommand(string UserId) : ICommand;
