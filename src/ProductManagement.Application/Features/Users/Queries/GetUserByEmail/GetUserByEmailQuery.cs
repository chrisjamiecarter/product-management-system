using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

public sealed record class GetUserByEmailQuery(string Email) : IQuery<GetUserByEmailQueryResponse>;
