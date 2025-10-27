using Node.Application.Features.Users.Commands;
using Node.Shared.Consts;
using FastEndpoints;
using MediatR;

namespace Node.Server.Controllers.Users;
public sealed class DeleteUserEndpoint(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete(ApiRoutes.Users.DeleteUser);
        Roles(RoleConsts.User);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<Guid>("id", isRequired: true);

        await mediator.Send(new DeleteUserCommand
        {
            UserRefId = userId!
        }, ct);

        await SendNoContentAsync(ct);
    }
}