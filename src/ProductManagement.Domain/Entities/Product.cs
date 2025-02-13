using ProductManagement.Domain.Primitives;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Entities;

/// <summary>
/// Represents a product entity.
/// </summary>
public class Product : AggregateRoot
{
    public Product(Guid id, ProductName name, string description, ProductPrice price) : base(id)
    {
        Name = name;
        Description = description;
        IsActive = true;
        AddedOnUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        Price = price;
    }

    public ProductName Name { get; set; }
    
    public string Description { get; set; }

    public bool IsActive { get; set; }

    public DateOnly AddedOnUtc { get; private set; }

    public ProductPrice Price { get; set; }
}
