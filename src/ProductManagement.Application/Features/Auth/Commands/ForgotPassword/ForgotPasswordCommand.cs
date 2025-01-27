using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email,
                                           string ResetPasswordUrl) : ICommand;
