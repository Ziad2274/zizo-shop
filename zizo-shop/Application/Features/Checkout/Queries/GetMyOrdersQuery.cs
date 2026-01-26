using MediatR;
using zizo_shop.Application.DTOs.Order;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Checkout.Queries
{
    public record GetMyOrdersQuery() : IRequest<List<OrderDto>>;

}
