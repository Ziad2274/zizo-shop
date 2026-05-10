using FluentValidation;
namespace zizo_shop.Application.Features.Categories.Commands
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100).Matches("^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric with hyphens.");
        }
    }
}
