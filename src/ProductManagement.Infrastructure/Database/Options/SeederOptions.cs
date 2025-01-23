using ProductManagement.Infrastructure.Database.Models;

namespace ProductManagement.Infrastructure.Email.Options;

internal class SeederOptions
{
    public bool SeedDatabase { get; set; }

    public int NumberOfProducts { get; set; }

    public SeedUser Owner { get; set; } = new SeedUser();

    public SeedUser Admin { get; set; } = new SeedUser();

    public SeedUser User { get; set; } = new SeedUser();
}
