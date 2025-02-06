using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Infrastructure.Constants;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Configurations;

/// <summary>
/// Configures an <see cref="AuditLog"/> table definition in the database.
/// </summary>
internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable(Tables.AuditLog, Schemas.Audit);

        builder.HasKey(pk => pk.Id);

        builder.Property(p => p.Message).IsRequired();

        builder.Property(p => p.Level).IsRequired();

        builder.Property(p => p.TimeStamp).IsRequired();
    }
}
