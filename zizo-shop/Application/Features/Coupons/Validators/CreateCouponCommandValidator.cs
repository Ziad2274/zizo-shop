using FluentValidation;
using zizo_shop.Application.Features.Coupons.Commands;

namespace zizo_shop.Application.Features.Coupons.Validators
{
    public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(20)
                .Matches("^[A-Z0-9]+$")
                .WithMessage("Code must be uppercase letters and numbers only.");

            RuleFor(x => x.DiscountPercent)
                .InclusiveBetween(1, 100)
                .WithMessage("Discount must be between 1 and 100 percent.");

            RuleFor(x => x.MaxUses)
                .GreaterThan(0)
                .WithMessage("Max uses must be at least 1.");

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Expiry date must be in the future.");

            RuleFor(x => x.MinOrderAmount)
                .GreaterThan(0)
                .When(x => x.MinOrderAmount.HasValue)
                .WithMessage("Minimum order amount must be greater than 0.");

            RuleFor(x => x.MaxDiscountAmount)
                .GreaterThan(0)
                .When(x => x.MaxDiscountAmount.HasValue)
                .WithMessage("Max discount amount must be greater than 0.");
        }
    }
}

