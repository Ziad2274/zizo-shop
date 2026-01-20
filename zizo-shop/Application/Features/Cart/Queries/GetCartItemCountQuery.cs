using MediatR;

namespace zizo_shop.Application.Features.Cart.Queries
{
    public record GetCartItemCountQuery() : IRequest<int>;
}
