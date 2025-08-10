using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.Infrastructure.Interfaces.Repositories;
using Fereira.Costa.Domain.ValueObjects;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Queries;

public sealed class GetPaginatedUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetPaginatedUserQuery, PaginatedResponse<User>>
{
    public Task<PaginatedResponse<User>> Handle(GetPaginatedUserQuery input, CancellationToken ct)
    {
        return userRepository.GetPaginatedUsersAsync(input.Skip, input.Take, input.Filter, ct);
    }
}

