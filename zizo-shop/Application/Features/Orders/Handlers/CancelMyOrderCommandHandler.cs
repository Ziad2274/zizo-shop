using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Orders.Commands;

namespace zizo_shop.Application.Features.Orders.Handlers
{
    public class CancelMyOrderCommandHandler : IRequestHandler<CancelMyOrderCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CancelMyOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task Handle(CancelMyOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var order =await _context.Orders.Include(o=>o.Items).FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId)
                ?? throw new KeyNotFoundException("Order not found.");
            order.Cancel();
            var procutIds=order.Items.Select(i => i.ProductId).ToList();
            var products = await _context.Products.Where(p => procutIds.Contains(p.Id)).ToListAsync(cancellationToken);

            foreach (var item in order.Items)
            {
                var product = await _context.Products.FindAsync(new object[] { item.ProductId }, cancellationToken) ?? throw new KeyNotFoundException("Product not found.");
                product.UpdateStock(item.Quantity);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
