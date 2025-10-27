using Node.Application.Features.Authentication.Queries;
using Node.Shared.Consts;
using FastEndpoints;
using MediatR;

namespace Node.Server.Controllers.Authentication;
public class GetProfileEndpoint(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get(ApiRoutes.Authentication.GetProfile);
        Roles(RoleConsts.User);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetProfileQuery(),cancellationToken: ct);

        await SendOkAsync(result, ct);
    }
}