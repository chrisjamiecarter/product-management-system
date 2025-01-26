using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Features.Users.Commands.ForgotPassword;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Identity;
using ProductManagement.Infrastructure.Errors;

namespace ProductManagement.Infrastructure.Handlers;

internal sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ForgotPasswordCommandHandler(IEmailService emailService, UserManager<ApplicationUser> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result.Failure(InfrastructureErrors.User.NotFound);
        }

        if (!user.EmailConfirmed)
        {
            return Result.Failure(InfrastructureErrors.User.EmailNotConfirmed);
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var uriBuilder = new UriBuilder(request.ResetPasswordUrl)
        {
            Query = $"code={resetToken}"
        };

        var resetLink = HtmlEncoder.Default.Encode(uriBuilder.ToString());
        await _emailService.SendEmailAsync(request.Email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.", cancellationToken);

        return Result.Success();
    }
}
