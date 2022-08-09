using FluentValidation;

namespace RapidPay.Application.Features.Card.Pay
{
    public class MakePaymentCommandValidator : AbstractValidator<MakePaymentCommand>
    {
        public MakePaymentCommandValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("CardNumber cannot be empty.");
            RuleFor(x => x.CardNumber).MinimumLength(15).WithMessage("Card number lenght must be 15.");
            RuleFor(x => x.CardNumber).MaximumLength(15).WithMessage("Card number lenght must be 15.");

            /* Only digits validation for CardNumber */
            RuleFor(x => x.CardNumber)
                .Custom((x, context) =>
                {
                    var isMatch = System.Text.RegularExpressions.Regex.IsMatch(x, "^[0-9]*$");

                    if (!isMatch)
                        context.AddFailure($"Card number must be composed by digits only.");
                });
        }
    }
}
