﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Database.Constants;

namespace ProductManagement.Infrastructure.Database.Configurations;

/// <summary>
/// Configures a <see cref="Product"/> table definition in the database.
/// </summary>
internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product", SchemaConstants.CoreSchema);

        builder.HasKey(pk => pk.Id);

        builder.Property(p => p.Name)
            .HasConversion(
                name => name.Value,
                value => ProductName.Create(value).Value)
            .IsRequired();

        builder.Property(p => p.Description)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.AddedOnUtc)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.Price)
            .HasConversion(
                price => price.Value,
                value => ProductPrice.Create(value).Value)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
