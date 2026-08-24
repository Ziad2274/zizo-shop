using FluentValidation;

namespace zizo_shop.Application.Features.Checkout.Commands
{
    public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(x => x.AddressId)
                .NotEmpty().WithMessage("Shipping address is required.");

            When(x => x.CouponCode != null, () =>
            RuleFor(x => x.CouponCode)
            .MaximumLength(20)
                .Matches("^[A-Z0-9]+$")
                .WithMessage("Invalid coupon code format.")
                .When(v => !string.IsNullOrWhiteSpace(v.CouponCode)
                ));

        
        }
    }
}
