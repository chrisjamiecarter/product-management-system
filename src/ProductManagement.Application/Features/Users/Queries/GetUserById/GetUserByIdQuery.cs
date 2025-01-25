using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(string UserId) : IQuery<GetUserByIdQueryResponse>;
