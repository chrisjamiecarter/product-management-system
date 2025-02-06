using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Infrastructure.Constants;

namespace ProductManagement.Infrastructure.Configurations;

/// <summary>
/// Configures an <see cref="IdentityRole"/> table definition in the database.
/// </summary>
internal class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable(Tables.Role, Schemas.Identity);
    }
}
