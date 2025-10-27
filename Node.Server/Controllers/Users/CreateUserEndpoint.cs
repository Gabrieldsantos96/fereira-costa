using FastEndpoints;
using Node.Application.Features.Users.Commands;
using Node.Application.Mappings;
using Node.Domain.Entities;
using Node.Shared.Consts;
using Node.Shared.Models;
using MediatR;

namespace Node.Server.Controllers.Users;
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