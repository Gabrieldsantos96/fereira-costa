using Node.Application.Features.Authentication.Commands;
using Node.Shared.Consts;
using Node.Shared.Models;
using FastEndpoints;
using MediatR;

namespace Node.Server.Controllers.Authentication;
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