using Fereira.Costa.Domain.Entities;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Queries;
public record GetUserQuery(Guid UserRefId) : IRequest<User>;

