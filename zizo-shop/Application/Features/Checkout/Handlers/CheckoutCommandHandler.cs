using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Checkout.Commands;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Checkout.Handlers
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CheckoutCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            var cart = await _context.Carts
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            var order = new Order { UserId = userId };
            foreach (var item in cart.Items)
            {
                if (item.Quantity > item.Product.StockQuantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {item.Product.Name}.");
                }
                item.Product.StockQuantity -= item.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.DiscountPrice ?? item.Product.Price
                });
            }          
            order.TotalPrice = order.Items.Sum(i => i.Price * i.Quantity);
            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
    }
}
