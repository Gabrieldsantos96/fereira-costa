using FastEndpoints;
using Fereira.Costa.Application.Features.Users.Queries;
using Fereira.Costa.Application.Mappings;
using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Shared.Consts;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Users;

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