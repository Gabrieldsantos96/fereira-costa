using Fereira.Costa.Domain.Entities;
using Fereira.Costa.Domain.Infrastructure.Interfaces.Repositories;
using Fereira.Costa.Domain.ValueObjects;
using Fereira.Costa.Shared.Models;
using MediatR;

namespace Fereira.Costa.Application.Features.Users.Commands;
public sealed class CreateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, MutationResult<User>>
{
    public async Task<MutationResult<User>> Handle(CreateUserCommand input, CancellationToken ct)
    {
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

        return MutationResult<User>.Ok("Usuário criado com sucesso", user);
    }
}
