using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Features.Users.Commands.RegisterUser;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Identity;

namespace ProductManagement.Infrastructure.Handlers;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserCommandHandler(IEmailService emailService, UserManager<ApplicationUser> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var uriBuilder = new UriBuilder(request.ConfirmEmailUrl)
            {
                Query = $"userId={user.Id}&code={confirmationToken}&returnUrl={request.ReturnUrl}"
            };

            var confirmationLink = HtmlEncoder.Default.Encode(uriBuilder.ToString());
            await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.", CancellationToken.None);

            return Result.Success();
        }
        else
        {
            var identityError = result.Errors.First();
            return identityError != null
                ? Result.Failure(new Error(identityError.Code, identityError.Description))
                : Result.Failure(InfrastructureErrors.User.NotRegistered);
        }
    }
}
