namespace ProductManagement.Infrastructure.EmailRender.Views.Emails.ChangeEmailConfirmation;

/// <summary>
/// Represents the model for a change email confirmation view.
/// </summary>
/// <param name="ChangeEmailConfirmationLink">The url for confirming an email change.</param>
public sealed record ChangeEmailConfirmationViewModel(string ChangeEmailConfirmationLink);
