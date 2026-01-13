using MediatR;

namespace zizo_shop.Application.Features.Checkout.Commands
{
    public record CheckoutCommand() : IRequest<Guid>;

}
