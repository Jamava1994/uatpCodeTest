using FluentValidation;

namespace RapidPay.Application.Features.User.Authenticate
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username cannot be empty.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty.");
        }
    }
}
