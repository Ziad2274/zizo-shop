using MediatR;
namespace zizo_shop.Application.Features.Orders.Commands
{
    public record UpdateOrderStatusCommand(Guid OrderId, string Action) : IRequest;
    // Action: "pay" | "ship" | "deliver" | "cancel"
}
