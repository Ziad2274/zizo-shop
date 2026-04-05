using FluentValidation;

namespace zizo_shop.Application.Features.Products.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters.");
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.");
            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Product stock cannot be negative.");
            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
