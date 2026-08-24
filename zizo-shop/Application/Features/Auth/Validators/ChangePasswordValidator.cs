using FluentValidation;

namespace zizo_shop.Application.Features.Auth.Validators
{
    public class ChangePasswordValidator : AbstractValidator<Commands.ChangePasswordCommand>
    {
        public ChangePasswordValidator() { 
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");
        RuleFor(x => x.NewPassword)
                .Matches("[A-Z]")
                .WithMessage("New password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("New password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("New password must contain at least one digit.")
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        }
    }
}
