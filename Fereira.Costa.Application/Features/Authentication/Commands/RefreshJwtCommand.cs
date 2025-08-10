using Fereira.Costa.Shared.Models;
using Fereira.Costa.Shared.Validations;
using FluentValidation;
using MediatR;

namespace Fereira.Costa.Application.Features.Authentication.Commands;
public record RefreshJwtDto(string RefreshToken);
public record RefreshJwtCommand(string RefreshToken): IRequest<MutationResult<RefreshTokenResult>>;
public sealed class RefreshJwtCommandValidator : AbstractValidator<RefreshJwtCommand>
{
    public RefreshJwtCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("token"));
    }
}
public record RefreshTokenResult(string AccessToken, string RefreshTokenHash);
