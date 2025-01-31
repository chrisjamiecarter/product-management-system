﻿using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IUserService
{
    Task<Result> AddPasswordAsync(string userId, string password, CancellationToken cancellationToken = default);
    Task<Result> ChangeEmailAsync(string userId, string newEmail, AuthToken token, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> FindByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<bool>> HasPasswordAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsEmailConfirmedAsync(string userId, CancellationToken cancellationToken = default);
}
