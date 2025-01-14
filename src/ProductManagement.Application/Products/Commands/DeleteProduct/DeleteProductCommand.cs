﻿using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : ICommand;
