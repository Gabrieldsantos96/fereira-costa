using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.Exceptions;
using Fereira.Costa.Domain.Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Queries;
public sealed class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery input, CancellationToken ct)
    {
        var user = await userRepository.GetUserAsync(input.UserRefId, ct);

        return user is null ? throw new NotFoundException($"Usuário com o RefId: {input.UserRefId} não foi encontrado!") : user;
    }
}
