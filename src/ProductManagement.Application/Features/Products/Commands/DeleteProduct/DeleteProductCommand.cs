using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Represents a command to delete an existing product.
/// </summary>
public sealed record DeleteProductCommand(Guid ProductId) : ICommand;
