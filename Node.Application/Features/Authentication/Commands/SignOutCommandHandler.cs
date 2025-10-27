using Node.Domain.Infrastructure.Interfaces.Adapters;
using Node.Shared.Models;
using MediatR;

namespace Node.Application.Features.Authentication.Commands;
public sealed class SignOutCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<SignOutCommand, MutationResult<object>>
{
    public async Task<MutationResult<object>> Handle(SignOutCommand input, CancellationToken ct)
    {
        await authenticationService.DeleteRefreshTokenAsync(input.RefreshToken, ct);

        return MutationResult<object>.Ok("Usuário deslogado com sucesso", new object());
    }
}
