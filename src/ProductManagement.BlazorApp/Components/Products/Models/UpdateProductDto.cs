﻿using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Products.Queries.GetProductById;

namespace ProductManagement.BlazorApp.Components.Products.Models;

public class UpdateProductDto
{
    public UpdateProductDto(GetProductByIdQueryResponse product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        IsActive = product.IsActive;
        AddedOnUtc = product.AddedOnUtc;
        Price = product.Price;
    }

    [Required]
    [Editable(false)]
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; }

    [DataType(DataType.Text)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    [Editable(false)]
    [DataType(DataType.Date)]
    public DateOnly AddedOnUtc { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Range(0, 999_999_999_999_999.99)]
    public decimal Price { get; set; }
}
