using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.Infrastructure.Interfaces.Adapters;
using Fereira.Costa.Domain.Infrastructure.Interfaces.Repositories;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fereira.Costa.Application.Features.Authentication.Commands;
public sealed class SignUpCommandHandler(UserManager<User> userManager,IUserRepository userRepository, IPasswordHelper passwordHelper) : IRequestHandler<SignUpCommand, MutationResult<object>>
{
    public async Task<MutationResult<object>> Handle(SignUpCommand input, CancellationToken ct)
    {
        var cpfExists = await userRepository.CheckCpfAsync(input.Command.Cpf, ct);

        if (cpfExists != null) throw new ArgumentException(nameof(Cpf));

        var newUser = User.Create
            (
            naturalness: input.Command.Naturalness,
            nationality: input.Command.Nationality,
            birthDay: input.Command.BirthDay,
            cpf: new Cpf(input.Command.Cpf),
            email: input.Command.Email,
            userName: input.Command.Email,
            name: input.Command.Name,
            phone: input.Command.Phone,
            street: input.Command.Street,
            zipcode: input.Command.Zipcode,
            number: input.Command.Number,
            city: input.Command.City,
            geo: input.Command.Geolocation,
            emailConfirmed: true
            );

        newUser.PasswordHash = passwordHelper.GeneratePassword(newUser, input.Command.Password);




        var result = await userManager.CreateAsync(newUser);

        if (result.Succeeded)
        {
            return MutationResult<object>.Ok("Usuário criado com sucesso", new object());
        }

        throw new Exception("Erro ao tentar criar usuário");

    }
}


