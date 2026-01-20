using MediatR;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Checkout.Queries
{
    public record GetMyOrdersQuery() : IRequest<List<Order>>;

}
