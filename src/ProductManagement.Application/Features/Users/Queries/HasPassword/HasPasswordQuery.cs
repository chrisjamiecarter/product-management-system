using System.Security.Claims;
using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Queries.HasPassword;

public sealed record HasPasswordQuery(string UserId) : IQuery<HasPasswordQueryResponse>;
