using FluentValidation;

namespace zizo_shop.Application.Features.Auth.Validators
{
    public class UpdateProfileValidator:AbstractValidator<Commands.UpdateProfileCommand>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format.");
        }
    }
}
