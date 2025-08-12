using Fereira.Costa.Application.Mappings;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Queries;
public record GetUserQuery(Guid UserRefId) : IRequest<UserDto>;

