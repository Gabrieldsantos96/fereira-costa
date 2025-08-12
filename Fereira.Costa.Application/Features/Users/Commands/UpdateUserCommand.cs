using Fereira.Costa.Application.Mappings;
using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Shared.Models;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Commands;
public sealed class UpdateUserDto
{
    public DateTime BirthDay { get; set; }
    public string Naturalness { get; set; } = null!;
    public string Nationality { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public Name Name { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Zipcode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Geolocation { get; set; } = null!;
    public string Number { get; set; } = null!;
}

public sealed class UpdateUserCommand : IRequest<MutationResult<UserDto>>
{
    public Guid RefId { get; set; }
    public UpdateUserDto Command { get; set; } = null!;
}
