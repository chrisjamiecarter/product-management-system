using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Options;

/// <summary>
/// Defines the application seeding requirements.
/// </summary>
internal class SeederOptions
{
    public bool SeedDatabase { get; set; }

    public int NumberOfProducts { get; set; }

    public ICollection<SeedUser> SeedUsers { get; set; } = [];
}
