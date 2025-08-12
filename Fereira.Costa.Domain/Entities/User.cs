using Fereira.Costa.Domain.Exceptions;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Shared.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fereira.Costa.Domain.Entities;

[Index(nameof(RefId), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(UserName), IsUnique = true)]
public class User : IdentityUser<int>
{
    public DateTime BirthDay { get; set; }
    public string Naturalness { get; set; } = null!;
    public string Nationality { get; set; } = null!;
    public Guid RefId { get; set; }
    public Name Name { get; set; }
    public Cpf? Cpf { get; set; }
    public Address? Address { get; set; }
    public string Phone { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User() { }
    public static User Create(
        DateTime birthDay,
        string naturalness,
        string nationality,
        string email,
        string userName,
        Name name,
        Cpf cpf,
        string phone,
        string street,
        string zipcode,
        string city,
        string geo,
        string number,
        bool emailConfirmed = false
        )
    {
        var user = new User
        {
            BirthDay = birthDay,
            Naturalness = naturalness,
            Nationality = nationality,
            Email = email,
            UserName = userName,
            Name = name,
            Address = Address.Create(street: street, zipcode: zipcode, city: city, geo: geo, number: number),
            Phone = phone,
            Cpf = cpf,
            LockoutEnabled = false,
            EmailConfirmed = emailConfirmed,
            RefId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        new UserValidator().ValidateAndThrow(user);
        return user;
    }

    public void Update(
        DateTime? birthDay = null,
        string? naturalness = null,
        string? nationality = null,
        string? email = null,
        string? userName = null,
        Name? name = null,
        Cpf? cpf = null,
        Address? address = null,
        string? phone = null
        )
    {
        static T ThrowIfNull<T>(T? value, string property) where T : class
            => value ?? throw new DomainException(ValidationHelper.RequiredErrorMessage(property));

        static T ThrowIfStructNull<T>(T? value, string property) where T : struct
    => value ?? throw new DomainException(ValidationHelper.RequiredErrorMessage(property));

        if (birthDay is not null)
            BirthDay = ThrowIfStructNull(birthDay, nameof(BirthDay));

        if (naturalness is not null)
            Naturalness = ThrowIfNull(naturalness, nameof(Naturalness));

        if (nationality is not null)
            Nationality = ThrowIfNull(nationality, nameof(Nationality));

        if (email is not null)
            Email = ThrowIfNull(email, nameof(Email));

        if (email is not null)
            Email = ThrowIfNull(email, nameof(Email));

        if (userName is not null)
            UserName = ThrowIfNull(userName, nameof(UserName));

        if (name is not null)
            Name = ThrowIfNull(name, nameof(Name));

        if (address is not null)
            Address = ThrowIfNull(address, nameof(Address));

        if (phone is not null)
            Phone = ThrowIfNull(phone, nameof(Phone));

        if (cpf is not null)
            Cpf = ThrowIfNull(cpf, nameof(Cpf));

        UpdatedAt = DateTime.UtcNow;

        new UserValidator().ValidateAndThrow(this);
    }
}
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("Email"))
            .EmailAddress().WithMessage("O email deve ser válido")
            .MaximumLength(256).WithMessage(ValidationHelper.MaxLengthErrorMessage("Email", 256));

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("Nome de usuário"))
            .MaximumLength(256).WithMessage(ValidationHelper.MaxLengthErrorMessage("Nome de usuário", 256));

        RuleFor(x => x.Name)
            .NotNull().WithMessage(ValidationHelper.RequiredErrorMessage("Nome"));

        RuleFor(x => x.Name.FirstName)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("Primeiro nome"))
            .MaximumLength(100).WithMessage(ValidationHelper.MaxLengthErrorMessage("Primeiro nome", 100)).When(x => x.Name != null);

        RuleFor(x => x.BirthDay)
           .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("Data de nascimento"));

        RuleFor(x => x.Name.LastName)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("Sobrenome"))
            .MaximumLength(100).WithMessage(ValidationHelper.MaxLengthErrorMessage("Sobrenome", 100)).When(x => x.Name != null);

        RuleFor(x => x.Address)
            .NotNull().WithMessage(ValidationHelper.RequiredErrorMessage("Endereço"));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("Telefone"))
            .MaximumLength(20).WithMessage(ValidationHelper.MaxLengthErrorMessage("Telefone", 20));

        RuleFor(x => x.RefId)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("RefId"));

        RuleFor(x => x.Cpf)
            .NotNull().WithMessage(ValidationHelper.RequiredErrorMessage("CPF"));
    }
}