using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Represents a command to update an existing product.
/// </summary>
public sealed record UpdateProductCommand(Guid ProductId,
                                          string Name,
                                          string Description,
                                          bool IsActive,
                                          decimal Price) : ICommand;
