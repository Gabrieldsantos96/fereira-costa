using Fereira.Costa.Application.Features.Authentication.Commands;
using Fereira.Costa.Shared.Consts;
using Fereira.Costa.Shared.Models;
using FastEndpoints;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Authentication;
public class SignUpEndpoint(IMediator mediator) : Endpoint<SignUpDto, MutationResult<object>>
{
    public override void Configure()
    {
        Post(ApiRoutes.Authentication.Register);
        AllowAnonymous();
    }
    public override async Task HandleAsync(SignUpDto req, CancellationToken ct)
    {
        var result = await mediator.Send(new SignUpCommand()
        {
            Command = req
        }, ct);

        await SendOkAsync(result, ct);
    }
}