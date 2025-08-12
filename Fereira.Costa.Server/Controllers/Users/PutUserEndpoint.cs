using FastEndpoints;
using Fereira.Costa.Application.Features.Users.Commands;
using Fereira.Costa.Application.Mappings;
using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Shared.Consts;
using Fereira.Costa.Shared.Models;
using MediatR;

namespace Fereira.Costa.Server.Controllers.Users;
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