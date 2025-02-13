namespace ProductManagement.Infrastructure.EmailRender.Views.Shared;

/// <summary>
/// Represents the model for an email button view.
/// </summary>
/// <param name="Text">The text for the button.</param>
/// <param name="Url">The url for the button.</param>
public record EmailButtonViewModel(string Text, string Url);
