using Node.Application.Features.Authentication.Commands;
using Node.Shared.Consts;
using Node.Shared.Models;
using FastEndpoints;
using MediatR;

namespace Node.Server.Controllers.Authentication;
public class RefreshJwtEndpoint(IMediator mediator) : Endpoint<RefreshJwtDto, MutationResult<RefreshTokenResult>>
{
    public override void Configure()
    {
        Post(ApiRoutes.Authentication.RefreshJwt);
        AllowAnonymous();
    }
    public override async Task HandleAsync(RefreshJwtDto req, CancellationToken ct)
    {
        var refreshJwt = await mediator.Send(new RefreshJwtCommand(req.RefreshToken), ct);

        await SendOkAsync(refreshJwt, ct);
    }
}