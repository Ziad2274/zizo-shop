using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Orders.Commands;

namespace zizo_shop.Application.Features.Orders.Handlers
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateOrderStatusCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new KeyNotFoundException("Order not found.");

            switch (request.Action.ToLower())
            {
                case "pay":     order.MarkAsPaid();    break;
                case "ship":    order.MarkAsShipped(); break;
                case "deliver": order.MarkAsDelivered(); break;
                case "cancel":  order.Cancel();        break;
                default: throw new ArgumentException($"Unknown action '{request.Action}'. Use: pay, ship, deliver, cancel.");
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
