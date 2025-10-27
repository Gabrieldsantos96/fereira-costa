using Node.Application.Mappings;
using Node.Domain.Entities;
using Node.Domain.Exceptions;
using Node.Domain.Infrastructure.Interfaces.Repositories;
using Node.Domain.ValueObjects;
using Node.Shared.Models;
using MediatR;

namespace Node.Application.Features.Users.Commands;
public sealed class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, MutationResult<UserDto>>
{
    public async Task<MutationResult<UserDto>> Handle(UpdateUserCommand input, CancellationToken ct)
    {
        var existing = await userRepository.GetUserAsync(input.RefId, ct)
                       ?? throw new NotFoundException($"Usuário com Id '{input.RefId}' não encontrado.");

        var cpfExists = await userRepository.CheckCpfAsync(input.Command.Cpf, ct);

        if (cpfExists != null && input.Command.Cpf != existing!.Cpf!.Value)
        {
            throw new ArgumentException(nameof(Cpf));
        }
        var userAddress = Address.Create(street: input.Command.Street, zipcode: input.Command.Zipcode, city: input.Command.City, geo: input.Command.Geolocation, number: input.Command.Number);

        existing.Update(
            input.Command.BirthDay,
            input.Command.Naturalness,
            input.Command.Nationality,
            input.Command.Email,
            input.Command.UserName,
            input.Command.Name,
            new Cpf(input.Command.Cpf),
            userAddress,
            input.Command.Phone
        );
        await userRepository.UpsertUserAsync(existing, ct);

        return MutationResult<UserDto>.Ok("Usuário atualizado com sucesso", existing.ToApplication());
    }
}
