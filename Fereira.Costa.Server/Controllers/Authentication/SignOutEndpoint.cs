using Fereira.Costa.Application.Features.Authentication.Commands;
using Fereira.Costa.Shared.Consts;
using Fereira.Costa.Shared.Models;
using FastEndpoints;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Authentication;
public class SignOutEndpoint(IMediator mediator) : Endpoint<SignOutDto, MutationResult<object>>
{
    public override void Configure()
    {
        Post(ApiRoutes.Authentication.SignOut);
        Roles(RoleConsts.User);
    }

    public override async Task HandleAsync(SignOutDto req, CancellationToken ct)
    {
        var result = await mediator.Send(new SignOutCommand(req.RefreshToken), ct);

        await SendOkAsync(result, ct);
    }
}