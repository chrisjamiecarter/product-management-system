using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId,
                                         string Token) : ICommand;
