﻿using System.Security.Claims;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IAuthService
{
    Task<Result> AddToRoleAsync(string userId, string? role, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(string userId, AuthToken token, CancellationToken cancellationToken = default);
    Task<Result<AuthToken>> GenerateEmailChangeTokenAsync(string userId, string newEmail, CancellationToken cancellationToken = default);
    Task<Result<AuthToken>> GenerateEmailConfirmationTokenAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<AuthToken>> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> GetCurrentUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> PasswordSignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default);
    Task<Result> RefreshSignInAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(string email, string? password, CancellationToken cancellationToken = default);
    Task<Result> ResetPasswordAsync(string email, string password, AuthToken token, CancellationToken cancellationToken = default);
    Task<Result> SignInAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> SignOutAsync(CancellationToken cancellationToken = default);
    Task<Result> ValidateSecurityStampAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
