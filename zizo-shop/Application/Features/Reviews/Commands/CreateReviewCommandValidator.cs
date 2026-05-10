using FluentValidation;
namespace zizo_shop.Application.Features.Reviews.Commands
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.Comment).NotEmpty().MaximumLength(1000);
        }
    }
}
