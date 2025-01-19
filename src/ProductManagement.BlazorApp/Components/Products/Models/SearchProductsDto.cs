using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Products.Queries.GetProductById;

namespace ProductManagement.BlazorApp.Components.Products.Models;

public class SearchProductsDto
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
