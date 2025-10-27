using Node.Domain.Entities;
using Node.Domain.ValueObjects;

namespace Node.Application.Mappings;

public class UserDto(
    Guid refId,
    Name name,
    string email,
    DateTime birthDay,
    string naturalness,
    string nationality,
    Cpf cpf,
    Address address,
    string phone,
    string userName,
    DateTime createdAt,
    DateTime updatedAt)
{
    public Guid RefId { get; set; } = refId;
    public Name Name { get; set; } = name;

    public string UserName { get; set; } = userName;
    public string Email { get; set; } = email;
    public DateTime BirthDay { get; set; } = birthDay;
    public string Naturalness { get; set; } = naturalness;
    public string Nationality { get; set; } = nationality;
    public Cpf Cpf { get; set; } = cpf;
    public Address Address { get; set; } = address;
    public string Phone { get; set; } = phone;
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
}

public static class UserMapping
{
    public static UserDto ToApplication(this User user)
    {
        return new UserDto(
            refId: user.RefId,
            name: user.Name,
            email: user.Email!,
            birthDay: user.BirthDay,
            naturalness: user.Naturalness,
            nationality: user.Nationality,
            cpf: user.Cpf!,
            address: user.Address,
            phone: user.Phone,
            userName: user.UserName ?? string.Empty,
            createdAt: user.CreatedAt,
            updatedAt: user.UpdatedAt
        );
    }
}
