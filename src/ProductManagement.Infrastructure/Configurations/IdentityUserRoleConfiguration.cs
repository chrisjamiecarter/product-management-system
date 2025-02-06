using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Infrastructure.Constants;

namespace ProductManagement.Infrastructure.Configurations;

/// <summary>
/// Configures an <see cref="IdentityUserRole"/> table definition in the database.
/// </summary>
internal class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable(Tables.ApplicationUserRole, Schemas.Identity);
    }
}
