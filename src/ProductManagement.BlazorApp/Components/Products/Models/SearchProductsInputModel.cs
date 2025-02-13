using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Products.Models;

/// <summary>
/// Represents the input model for searching products.
/// </summary>
public class SearchProductsInputModel
{
    [DataType(DataType.Text)]
    public string? SearchName { get; set; }

    public bool? SearchIsActive { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? SearchFromAddedOnDateUtc { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? SearchToAddedOnDateUtc { get; set; }

    [DataType(DataType.Currency)]
    public decimal? SearchFromPrice { get; set; }

    [DataType(DataType.Currency)]
    public decimal? SearchToPrice { get; set; }

    public string? SortColumn { get; set; }

    public string? SortOrder { get; set; }
}
