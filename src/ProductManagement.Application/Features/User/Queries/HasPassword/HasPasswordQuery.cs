using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.User.Queries.HasPassword;

public sealed record HasPasswordQuery(string UserId) : IQuery<HasPasswordQueryResponse>;
