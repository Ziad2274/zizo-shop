using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Checkout.Commands;
using zizo_shop.Domain.Entities;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Checkout.Handlers
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<Guid> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("User is not authenticated.");

            // Validate that address belongs to the current user
            var addressExists = await _context.Address
                .AnyAsync(a => a.Id == request.AddressId && a.UserId == userId, cancellationToken);
            if (!addressExists)
                throw new KeyNotFoundException("Shipping address not found.");

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("Your cart is empty.");

            var order = new Order(userId) { AddressId = request.AddressId };

            foreach (var item in cart.Items)
            {
                item.Product.RemoveStock(item.Quantity);
                order.AddItem(item.Product, item.Quantity);
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync(cancellationToken);

            // Fire-and-forget email using the user's email address
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user?.Email != null)
            {
                var email = user.Email;
                var orderId = order.Id;
                BackgroundJob.Enqueue<IEmailService>(svc =>
                    svc.SendEmailAsync(
                        email,
                        "Order Confirmation",
                        $"<h2>Thank you for your order!</h2><p>Your order <strong>{orderId}</strong> has been placed successfully.</p>"));
            }

            return order.Id;
        }
    }
}
