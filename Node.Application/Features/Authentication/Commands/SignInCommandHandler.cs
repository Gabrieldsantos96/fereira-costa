using Node.Domain.Entities;
using Node.Shared.Validations;
using Node.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using CommonSignInResult = Node.Application.Features.Authentication.Commands.SignInResult;
using Node.Domain.Infrastructure.Interfaces.Adapters;
using MediatR;

namespace Node.Application.Features.Authentication.Commands;
public sealed class SignInCommandHandler(IAuthenticationService authService, IAuthenticationService authenticationService, IJwtService jwtService, SignInManager<User> signInManager, IClaimsService claimsService) : IRequestHandler<SignInCommand, MutationResult<CommonSignInResult>>
{
    public async Task<MutationResult<CommonSignInResult>> Handle(SignInCommand input, CancellationToken ct)
    {
        var user = await authService.GetUserAsync(input.Email, ct)
         ?? throw new AuthenticationException(ValidationMessages.DefaultAuthenticationError);

        var result = await signInManager.CheckPasswordSignInAsync(user, input.Password, false);

        if (result.Succeeded)
        {
            var (accessToken, refreshTokenHash) = jwtService.CreateJwt(claimsService.GenerateClaims(user));

            await authenticationService.CreateRefreshTokenAsync(new RefreshToken()
            {
                UserRefId = user.RefId,
                TokenHash = refreshTokenHash
            }, ct);

            return MutationResult<CommonSignInResult>.Ok("Usuário autenticado com sucesso", new CommonSignInResult(accessToken, refreshTokenHash));
        }

        if (result.IsNotAllowed) throw new Exception(ValidationMessages.IsNotAllowed);

        if (result.IsLockedOut) throw new ArgumentException(ValidationMessages.UserLockedOut);

        throw new ArgumentException(ValidationMessages.DefaultAuthenticationError);
    }
}


