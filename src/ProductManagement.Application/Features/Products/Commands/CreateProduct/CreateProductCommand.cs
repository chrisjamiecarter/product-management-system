using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Represents a command to create a new product.
/// </summary>
public sealed record CreateProductCommand(string Name,
                                          string Description,
                                          decimal Price) : ICommand;
