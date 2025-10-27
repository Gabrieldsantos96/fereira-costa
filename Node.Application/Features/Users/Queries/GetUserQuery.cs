using Node.Application.Mappings;
using MediatR;

namespace Node.Application.Features.Users.Queries;
public record GetUserQuery(Guid UserRefId) : IRequest<UserDto>;

