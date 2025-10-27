using Node.Domain.ValueObjects;
using Node.Shared.Models;
using MediatR;

namespace Node.Application.Features.Authentication.Commands;
public record SignUpDto
{
    public DateTime BirthDay { get; set; }
    public string Naturalness { get; set; } = null!;
    public string Nationality { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public Name Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Zipcode { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Geolocation { get; set; } = null!;
    public string Password { get; set; } = null!;
}
public record SignUpCommand : IRequest<MutationResult<object>>
{
    public SignUpDto Command { get; set; } = null!;
}
