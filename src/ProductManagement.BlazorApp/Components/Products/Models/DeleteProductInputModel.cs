using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Features.Products.Queries.GetProductById;

namespace ProductManagement.BlazorApp.Components.Products.Models;

/// <summary>
/// Represents the input model for deleting an existing product.
/// </summary>
public class DeleteProductinputModel
{
    public DeleteProductinputModel(GetProductByIdQueryResponse product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        IsActive = product.IsActive;
        AddedOnUtc = product.AddedOnUtc;
        Price = product.Price;
    }

    [Editable(false)]
    public Guid Id { get; set; }

    [Editable(false)]
    [DataType(DataType.Text)]
    public string Name { get; set; }

    [Editable(false)]
    [DataType(DataType.Text)]
    public string Description { get; set; } = string.Empty;

    [Editable(false)]
    public bool IsActive { get; set; }

    [Editable(false)]
    [DataType(DataType.Date)]
    public DateOnly AddedOnUtc { get; set; }

    [Editable(false)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
}
