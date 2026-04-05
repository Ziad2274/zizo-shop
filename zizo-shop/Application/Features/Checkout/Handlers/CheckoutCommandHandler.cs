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
        private readonly UserManager<ApplicationUser> _userManger;
        
        public CheckoutCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IEmailService emailService, UserManager<ApplicationUser> userManger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _emailService = emailService;
            _userManger = userManger;
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
            if (cart == null || !cart.Items.Any()) {
                throw new Exception("Cart not found for the user.");
            }
            var order = new Order(userId) { 
            AddressId = request.AddressId,
            } ;
            foreach (var item in cart.Items)
            {
                
                item.Product.RemoveStock(item.Quantity);
                order.AddItem(item.Product,item.Quantity);
            }          
            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync(cancellationToken);
                var user = await _userManger.FindByIdAsync(userId.ToString());
            BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(
                user.Email, "Order Confirmation", $"Your order with ID {order.Id} has been placed successfully."));
            return order.Id;
        }
    }
}
