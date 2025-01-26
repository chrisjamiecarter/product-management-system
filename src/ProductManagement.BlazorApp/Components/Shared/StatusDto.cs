using ProductManagement.BlazorApp.Enums;

namespace ProductManagement.BlazorApp.Components.Shared;

public class StatusDto
{
    public string? Message { get; set; }

    public MessageLevel? Level { get; set; }
}
