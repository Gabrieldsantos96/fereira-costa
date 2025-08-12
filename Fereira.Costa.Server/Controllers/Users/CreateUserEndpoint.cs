using FastEndpoints;
using Fereira.Costa.Application.Features.Users.Commands;
using Fereira.Costa.Application.Mappings;
using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Shared.Consts;
using Fereira.Costa.Shared.Models;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Users;
public sealed class CreateUserEndpoint(IMediator mediator) : Endpoint<CreateUserDto, MutationResult<UserDto>>
{
    public override void Configure()
    {
        Post(ApiRoutes.Users.CreateUser);
        Roles(RoleConsts.User);
    }

    public override async Task HandleAsync(CreateUserDto req, CancellationToken ct)
    {

        var result = await mediator.Send(new CreateUserCommand
        {
            Command = req
        }, ct);

        await SendOkAsync(result, ct);
    }
}