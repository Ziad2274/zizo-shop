using FluentValidation;
using zizo_shop.Application.Features.Payments.Commands;

namespace zizo_shop.Application.Features.Payments.Validators
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required.");
            RuleFor(x => x.Provider).NotEmpty()
                .Must(x => new[] {"Mobile Wallet", "InstaPay", "Fawry", "PayPal", "Stripe", "CreditCard" }.Contains(x))
                .WithMessage("Payment provider is required in \"Mobile Wallet, InstaPay , Fawry, PayPal, Stripe, CreditCard.\"");
        }
    }
}
