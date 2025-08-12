using Fereira.Costa.Application.Mappings;
using Fereira.Costa.Domain.Infrastructure.Interfaces.Repositories;
using Fereira.Costa.Domain.ValueObjects;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Queries;

public sealed class GetPaginatedUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetPaginatedUserQuery, PaginatedResponse<UserDto>>
{
    public async Task<PaginatedResponse<UserDto>> Handle(GetPaginatedUserQuery input, CancellationToken ct)
    {
        var result = await userRepository.GetPaginatedUsersAsync(input.Skip, input.Take, input.Filter, ct);

        var updatedData = result.Data.Select(s => s.ToApplication()).ToList();

        return new PaginatedResponse<UserDto>(updatedData, result.TotalItems, result.CurrentPage, result.TotalPages);
    }
}

