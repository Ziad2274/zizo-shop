using MediatR;

namespace zizo_shop.Application.Features.Cart.Commands
{
    public record ClearCartCommand() : IRequest<Unit>;
}
