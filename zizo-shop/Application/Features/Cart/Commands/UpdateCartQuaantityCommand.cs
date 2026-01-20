using MediatR;

namespace zizo_shop.Application.Features.Cart.Commands
{
    public record UpdateCartItemQuantityCommand(Guid ProductId, int Quantity) : IRequest<Unit>;
}
