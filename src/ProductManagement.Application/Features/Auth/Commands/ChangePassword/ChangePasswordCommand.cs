using System.Security.Claims;
using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ChangePassword;

public sealed record ChangePasswordCommand(ClaimsPrincipal Principal,
                                           string currentPassword,
                                           string newPassword) : ICommand;
