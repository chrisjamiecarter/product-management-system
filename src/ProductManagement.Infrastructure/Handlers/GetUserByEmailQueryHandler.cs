using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Features.Users.Queries.GetUserByEmail;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Identity;
using ProductManagement.Infrastructure.Errors;

namespace ProductManagement.Infrastructure.Handlers;

internal class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, GetUserByEmailQueryResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByEmailQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<GetUserByEmailQueryResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure<GetUserByEmailQueryResponse>(InfrastructureErrors.User.NotFound);
        }

        var response = new GetUserByEmailQueryResponse(user.Id, user.UserName, user.Email, user.EmailConfirmed);
        return Result.Success(response);
    }
}
