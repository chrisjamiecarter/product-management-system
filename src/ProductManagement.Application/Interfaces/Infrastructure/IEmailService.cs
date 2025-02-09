﻿using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IEmailService
{
    Task<Result> SendChangeEmailConfirmationAsync(string toEmailAddress, string changeEmailConfirmationLink, CancellationToken cancellationToken = default);
    Task<Result> SendEmailConfirmationAsync(string toEmailAddress, string emailConfirmationLink, CancellationToken cancellationToken = default);
    Task<Result> SendPasswordResetAsync(string toEmailAddress, string passwordResetLink, CancellationToken cancellationToken = default);
}
