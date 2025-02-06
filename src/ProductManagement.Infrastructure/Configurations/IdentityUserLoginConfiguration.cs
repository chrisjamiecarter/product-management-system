using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Infrastructure.Constants;

namespace ProductManagement.Infrastructure.Configurations;

/// <summary>
/// Configures an <see cref="IdentityUserLogin"/> table definition in the database.
/// </summary>
internal class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable(Tables.ApplicationUserLogin, Schemas.Identity);
    }
}
