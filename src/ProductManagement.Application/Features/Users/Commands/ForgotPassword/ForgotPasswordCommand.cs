using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email,
                                          string ResetPasswordUrl) : ICommand;
