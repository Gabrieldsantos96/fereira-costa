using Node.Domain.Entities;
using Node.Domain.Exceptions;
using Node.Domain.Infrastructure.Interfaces.Adapters;
using Node.Domain.Infrastructure.Interfaces.Repositories;
using Node.Shared.Models;
using MediatR;

namespace Node.Application.Features.Authentication.Commands;
public sealed class RefreshJwtCommandHandler(IJwtService jwtService, IClaimsService claimsService, IAuthenticationService authenticationService, IUserRepository userRepository): IRequestHandler<RefreshJwtCommand, MutationResult<RefreshTokenResult>>
{
    public async Task<MutationResult<RefreshTokenResult>> Handle(RefreshJwtCommand input, CancellationToken ct)
    {

        var currentRefreshToken = await authenticationService.ValidateRefreshTokenAsync(input.RefreshToken, ct);

        var user = await userRepository.GetUserAsync(currentRefreshToken.UserRefId, ct) ?? throw new NotFoundException();

        var claims = claimsService.GenerateClaims(user);

        var (accessToken, refreshTokenHash) = jwtService.CreateJwt(claims);

        await authenticationService.RenewRefreshTokenAsync(currentRefreshToken, newToken: new RefreshToken()
        {
            TokenHash = refreshTokenHash,
            UserRefId = user.RefId
        }, ct);

        return MutationResult<RefreshTokenResult>.Ok("Usuário autenticado com sucesso", new RefreshTokenResult(accessToken, refreshTokenHash));
    }
}

