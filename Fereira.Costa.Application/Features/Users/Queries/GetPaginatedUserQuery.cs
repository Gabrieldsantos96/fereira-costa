using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.ValueObjects;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Queries;

public sealed class PaginationDto
{
    public int Skip { get; set; } = 1;
    public int Take { get; set; } = 20;
    public string? Filter { get; set; }
}
public record GetPaginatedUserQuery(int Skip = 0, int Take = 20, string? Filter = null) : IRequest<PaginatedResponse<User>>;
