using FluentValidation;

namespace zizo_shop.Application.Features.Checkout.Commands
{
    public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(x => x.AddressId)
                .NotEmpty().WithMessage("Shipping address is required.");
          
        }
    }
}
