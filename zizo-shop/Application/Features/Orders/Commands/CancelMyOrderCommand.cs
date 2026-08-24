using MediatR;

namespace zizo_shop.Application.Features.Orders.Commands
{
    public record CancelMyOrderCommand(Guid OrderId) : IRequest;

}
