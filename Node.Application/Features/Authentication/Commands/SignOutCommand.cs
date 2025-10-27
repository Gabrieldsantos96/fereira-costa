using Node.Shared.Models;
using Node.Shared.Validations;
using FluentValidation;
using MediatR;

namespace Node.Application.Features.Authentication.Commands;
public record SignOutDto(string RefreshToken);
public record SignOutCommand(string RefreshToken) : IRequest<MutationResult<object>>;
public sealed class SignOutCommandValidator : AbstractValidator<SignOutCommand>
{
    public SignOutCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("token"));
    }
}