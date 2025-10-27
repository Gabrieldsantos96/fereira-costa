using Node.Application.Features.Authentication.Commands;
using Node.Shared.Consts;
using Node.Shared.Models;
using FastEndpoints;
using MediatR;

namespace Node.Server.Controllers.Authentication;
public class SignOutEndpoint(IMediator mediator) : Endpoint<SignOutDto, MutationResult<object>>
{
    public override void Configure()
    {
        Post(ApiRoutes.Authentication.SignOut);
        AllowAnonymous();
    }

    public override async Task HandleAsync(SignOutDto req, CancellationToken ct)
    {
        var result = await mediator.Send(new SignOutCommand(req.RefreshToken), ct);

        await SendOkAsync(result, ct);
    }
}