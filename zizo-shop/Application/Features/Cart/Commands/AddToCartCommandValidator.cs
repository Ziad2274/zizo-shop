using FluentValidation;

namespace zizo_shop.Application.Features.Cart.Commands
{
    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThan(100).WithMessage("Quantity must be less than 100 at once.")
                ;
        }
    }
}
