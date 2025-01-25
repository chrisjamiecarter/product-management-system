using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name,
                                          string Description,
                                          decimal Price) : ICommand;
