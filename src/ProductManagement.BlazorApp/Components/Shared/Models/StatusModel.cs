using ProductManagement.BlazorApp.Enums;

namespace ProductManagement.BlazorApp.Components.Shared.Models;

/// <summary>
/// Represents a status message with an optional message level.
/// </summary>
public sealed class StatusModel
{
    public static readonly StatusModel None = new(null, null);

    public StatusModel(string? message, MessageLevel? level)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            if (level is null)
            {
                level = MessageLevel.Primary;
            }

            Message = message;
            Level = level;
        }
    }

    public string? Message { get; set; }

    public MessageLevel? Level { get; set; }
}
