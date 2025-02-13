namespace ProductManagement.Infrastructure.EmailRender.Views.Emails.EmailConfirmation;

/// <summary>
/// Represents the model for an email confirmation view.
/// </summary>
/// <param name="EmailConfirmationLink">The url for confirming an email.</param>
public sealed record EmailConfirmationViewModel(string EmailConfirmationLink);
