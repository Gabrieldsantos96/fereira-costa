using FastEndpoints;
using Node.Application.Features.Users.Queries;
using Node.Application.Mappings;
using Node.Domain.Entities;
using Node.Domain.ValueObjects;
using Node.Shared.Consts;
using MediatR;

namespace Node.Server.Controllers.Users;

public sealed class GetPaginatedProductsEndpoint(IMediator mediator) : EndpointWithoutRequest<PaginatedResponse<UserDto>>
{
    public override void Configure()
    {
        Get(ApiRoutes.Users.GetPaginatedUsers);
        Roles(RoleConsts.User);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int skip = Query<int>("skip", isRequired: false);
        int take = Query<int>("take", isRequired: false);
        string? filter = Query<string>("filter", isRequired: false);

        skip = skip < 0 ? 0 : skip;
        take = take <= 0 ? 10 : take;

        var result = await mediator.Send(new GetPaginatedUserQuery(skip, take, filter), ct);

        await SendOkAsync(result, ct);
    }
}