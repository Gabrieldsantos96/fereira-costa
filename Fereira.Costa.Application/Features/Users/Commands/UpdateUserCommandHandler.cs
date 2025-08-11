using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.Exceptions;
using Fereira.Costa.Domain.Infrastructure.Interfaces.Repositories;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Shared.Models;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Commands;
public sealed class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, MutationResult<User>>
{
    public async Task<MutationResult<User>> Handle(UpdateUserCommand input, CancellationToken ct)
    {
        var existing = await userRepository.GetUserAsync(input.RefId, ct)
                       ?? throw new NotFoundException($"Usuário com Id '{input.RefId}' não encontrado.");

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

        return MutationResult<User>.Ok("Usuário atualizado com sucesso", existing);
    }
}
