using FluentValidation;
namespace zizo_shop.Application.Features.Brands.Commands
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100).Matches("^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric with hyphens.");
        }
    }
}
