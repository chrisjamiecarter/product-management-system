using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId,
                                         string Token) : ICommand;
