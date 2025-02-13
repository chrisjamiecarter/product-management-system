using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Products.Models;

/// <summary>
/// Represents the input model for creating a new product.
/// </summary>
public class CreateProductInputModel
{
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Currency)]
    [Range(0.00, 999_999_999_999_999.99)]
    public decimal Price { get; set; }
}
