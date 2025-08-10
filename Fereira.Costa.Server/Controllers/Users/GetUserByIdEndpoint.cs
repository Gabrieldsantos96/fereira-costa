using Fereira.Costa.Application.Features.Users.Queries;
using Fereira.Costa.Shared.Consts;
using FastEndpoints;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Users;
public sealed class GetUserByIdEndpoint(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get(ApiRoutes.Users.GetUserById);
        Roles(RoleConsts.User);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<Guid>("id", isRequired: true);

        var result = await mediator.Send(new GetUserQuery(userId!), cancellationToken: ct);

        await SendOkAsync(result, ct);
    }
}