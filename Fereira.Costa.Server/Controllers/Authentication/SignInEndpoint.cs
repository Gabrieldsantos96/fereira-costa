using Fereira.Costa.Application.Features.Authentication.Commands;
using Fereira.Costa.Shared.Consts;
using Fereira.Costa.Shared.Models;
using FastEndpoints;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Authentication;
public class SignInEndpoint(IMediator mediator) : Endpoint<SignInDto, MutationResult<SignInResult>>
{
    public override void Configure()
    {
        Post(ApiRoutes.Authentication.SignIn);
        AllowAnonymous();
    }
    public override async Task HandleAsync(SignInDto req, CancellationToken ct)
    {
        var token = await mediator.Send(new SignInCommand()
        {
            Email = req.Email,
            Password = req.Password
        }, ct);

        await SendOkAsync(token, ct);
    }
}