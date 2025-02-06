using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Infrastructure.Constants;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Configurations;

/// <summary>
/// Configures an <see cref="ApplicationUser"/> table definition in the database.
/// </summary>
internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(Tables.ApplicationUser, Schemas.Identity);

        builder.HasMany(e => e.UserRoles)
               .WithOne()
               .HasForeignKey(fk => fk.UserId)
               .IsRequired();
    }
}
