using FluentValidation;
namespace zizo_shop.Application.Features.Address.Commands
{
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ZipCode).NotEmpty().MaximumLength(20);
        }
    }
}
