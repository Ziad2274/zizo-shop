using FluentValidation;
using zizo_shop.Application.Features.Auth.Commands;

namespace zizo_shop.Application.Features.Auth.Validators
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
