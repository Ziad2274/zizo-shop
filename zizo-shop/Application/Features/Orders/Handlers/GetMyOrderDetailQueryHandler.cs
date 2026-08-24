using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Order;
using zizo_shop.Application.Features.Orders.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Orders.Handlers
{
    public class GetMyOrderDetailQueryHandler : IRequestHandler<GetMyOrderDetailQuery, OrderDetailDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetMyOrderDetailQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<OrderDetailDto> Handle(GetMyOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var order = await _context.Orders.Include(o=>o.Items).FirstOrDefaultAsync(o=>o.Id==request.OrderId&&o.UserId==userId, cancellationToken) ?? throw new KeyNotFoundException("Order not found.");
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new KeyNotFoundException("User not found.");
            return new OrderDetailDto(
       order.Id,
       order.CreatedAt,
       order.TotalPrice,
       order.SubTotal,
       order.ShippingFee,
       order.DiscountAmount,
       order.CouponCode,
       order.Status.ToString(),
       user?.Email ?? "",
       order.AddressId,
       order.Items
           .Select(i => new OrderItemDto(
               i.ProductId,
               i.ProductName,
               i.Price,
               i.Quantity,
               i.Price * i.Quantity))
           .ToList());

        }
    }
}
