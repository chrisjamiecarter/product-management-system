namespace ProductManagement.Infrastructure.Email.Options;

internal class EmailOptions
{
    public string SmtpHost { get; set; } = string.Empty;
    
    public int SmtpPort { get; set; }

    public string SmtpUser { get; set; } = string.Empty;

    public string SmtpPassword { get; set; } = string.Empty;

    public string FromName { get; set; } = string.Empty;

    public string FromEmailAddress { get; set; } = string.Empty;
}
