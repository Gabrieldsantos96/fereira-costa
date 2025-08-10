using Fereira.Costa.Application.Features.Authentication.Commands;
using Fereira.Costa.Shared.Consts;
using Fereira.Costa.Shared.Models;
using FastEndpoints;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Authentication;
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