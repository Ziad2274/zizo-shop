using MediatR;

namespace zizo_shop.Application.Features.Checkout.Commands
{
    public record CheckoutCommand( Guid AddressId, string? CouponCode = null) : IRequest<Guid>;

}
