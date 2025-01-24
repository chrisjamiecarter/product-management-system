using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Services;
using ProductManagement.Application.Users.Commands.RegisterUser;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Identity;

namespace ProductManagement.Infrastructure.Users.Commands.RegisterUser;

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

        var userResult = await _userManager.CreateAsync(user, request.Password);
        //var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var uriBuilder = new UriBuilder(request.ConfirmEmailUrl)
        {
            Query = $"userId={user.Id}&code={confirmationToken}&returnUrl={request.ReturnUrl}"
        };

        var confirmationLink = HtmlEncoder.Default.Encode(uriBuilder.ToString());
        await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.", CancellationToken.None);

        // TODO: ERRORS.
        return userResult.Succeeded ? Result.Success() : Result.Failure(new Error("User.NotRegistered", "The user was not registered."));
    }
}
