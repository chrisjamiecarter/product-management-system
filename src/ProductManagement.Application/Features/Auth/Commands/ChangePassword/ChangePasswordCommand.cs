using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ChangePassword;

public sealed record ChangePasswordCommand(string UserId,
                                           string CurrentPassword,
                                           string UpdatedPassword) : ICommand;
