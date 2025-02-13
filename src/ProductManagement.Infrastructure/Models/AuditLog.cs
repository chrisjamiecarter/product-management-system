namespace ProductManagement.Infrastructure.Models;

/// <summary>
/// Represents an audit log entry.
/// </summary>
internal sealed class AuditLog
{
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;

    public string MessageTemplate { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public DateTime TimeStamp { get; set; }

    public string? Exception { get; set; }

    public string? Properties { get; set; }
}
