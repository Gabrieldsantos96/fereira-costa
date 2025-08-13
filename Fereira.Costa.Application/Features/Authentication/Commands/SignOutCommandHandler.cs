using Fereira.Costa.Domain.Infrastructure.Interfaces.Adapters;
using Fereira.Costa.Shared.Models;
using MediatR;

namespace Fereira.Costa.Application.Features.Authentication.Commands;
public sealed class SignOutCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<SignOutCommand, MutationResult<object>>
{
    public async Task<MutationResult<object>> Handle(SignOutCommand input, CancellationToken ct)
    {
        await authenticationService.DeleteRefreshTokenAsync(input.RefreshToken, ct);

        return MutationResult<object>.Ok("Usuário deslogado com sucesso", new object());
    }
}
