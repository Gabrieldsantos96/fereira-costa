using Node.Shared.Models;
using Node.Shared.Validations;
using FluentValidation;
using MediatR;

namespace Node.Application.Features.Authentication.Commands;
public record SignInDto(string Email, string Password);
public record SignInCommand : IRequest<MutationResult<SignInResult>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("senha"));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationHelper.RequiredErrorMessage("email"));
    }
}
public record SignInResult(string AccessToken, string RefreshToken);
