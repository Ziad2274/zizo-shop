using MediatR;

namespace zizo_shop.Application.Features.Cart.Commands
{
    public record AddToCartCommand(
        Guid ProductId,
        int Quantity
        ) : IRequest;
}
