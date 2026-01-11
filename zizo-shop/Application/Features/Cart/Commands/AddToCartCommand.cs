using MediatR;

namespace zizo_shop.Application.Features.Cart.Commands
{
    public record AddToWishlistCommand(
        Guid ProductId,
        int Quantity
        ) : IRequest;
}
