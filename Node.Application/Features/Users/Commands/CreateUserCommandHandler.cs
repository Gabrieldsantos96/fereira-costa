using Node.Application.Mappings;
using Node.Domain.Entities;
using Node.Domain.Infrastructure.Interfaces.Repositories;
using Node.Domain.ValueObjects;
using Node.Shared.Models;
using MediatR;

namespace Node.Application.Features.Users.Commands;
public sealed class CreateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, MutationResult<UserDto>>
{
    public async Task<MutationResult<UserDto>> Handle(CreateUserCommand input, CancellationToken ct)
    {
        var cpfExists = await userRepository.CheckCpfAsync(input.Command.Cpf, ct);

        if (cpfExists != null) throw new ArgumentException(nameof(Cpf));

        var user = User.Create(
            input.Command.BirthDay,
            input.Command.Naturalness,
            input.Command.Nationality,
            input.Command.Email,
            input.Command.UserName,
            input.Command.Name,
            new Cpf(input.Command.Cpf),
            input.Command.Phone,
            input.Command.Street,
            input.Command.Zipcode,
            input.Command.City,
            input.Command.Geolocation,
            input.Command.Number
            );

        await userRepository.UpsertUserAsync(user, ct);

        return MutationResult<UserDto>.Ok("Usuário criado com sucesso", user.ToApplication());
    }
}
