using Fereira.Costa.Application.Features.Authentication.Commands;
using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Shared.Models;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Commands;

public sealed class CreateUserDto
{
    public DateTime BirthDay { get; set; }
    public string Naturalness { get; set; } = null!;
    public string Nationality { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public Name Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Zipcode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Geolocation { get; set; } = null!;
    public string Number { get; set; } = null!;
}

public sealed class CreateUserCommand : IRequest<MutationResult<User>>
{
    public CreateUserDto Command { get; set; } = null!;
}
