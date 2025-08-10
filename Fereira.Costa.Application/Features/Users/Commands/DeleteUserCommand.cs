using MediatR;

namespace Fereira.Costa.Application.Features.Users.Commands;
public sealed class DeleteUserCommand : IRequest
{
    public Guid UserRefId { get; set;  }
}
