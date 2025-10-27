using FastEndpoints;
using Node.Application.Features.Users.Commands;
using Node.Application.Mappings;
using Node.Domain.Entities;
using Node.Shared.Consts;
using Node.Shared.Models;
using MediatR;

namespace Node.Server.Controllers.Users;
public sealed class UpdateUserEndpoint(IMediator mediator) : Endpoint<UpdateUserDto, MutationResult<UserDto>>
{
    public override void Configure()
    {
        Put(ApiRoutes.Users.UpdateUser);
        Roles(RoleConsts.User);
    }

    public override async Task HandleAsync(UpdateUserDto req, CancellationToken ct)
    {
        var userId = Route<Guid>("id", isRequired: true);

        var result = await mediator.Send(new UpdateUserCommand
        {
            RefId = userId!,
            Command = req
        }, ct);

        await SendOkAsync(result, ct);
    }
}