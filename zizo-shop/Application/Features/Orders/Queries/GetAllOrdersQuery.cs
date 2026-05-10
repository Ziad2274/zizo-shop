using MediatR;
using zizo_shop.Application.DTOs.Order;
namespace zizo_shop.Application.Features.Orders.Queries
{
    public record GetAllOrdersQuery() : IRequest<List<OrderDetailDto>>;
}
