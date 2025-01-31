using System.Security.Claims;
using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(string UserId) : IQuery<GetCurrentUserQueryResponse>;
