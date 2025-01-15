using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    decimal Price) : ICommand;
