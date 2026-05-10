using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Order;
using zizo_shop.Application.Features.Orders.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Orders.Handlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetOrderByIdQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        { _context = context; _userManager = userManager; }

        public async Task<OrderDetailDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var o = await _context.Orders.Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new KeyNotFoundException("Order not found.");
            var user = await _userManager.FindByIdAsync(o.UserId.ToString());
            return new OrderDetailDto(
                o.Id, o.CreatedAt, o.TotalPrice, o.SubTotal, o.ShippingFee,
                o.Status.ToString(), user?.Email ?? "unknown", o.AddressId,
                o.Items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.Price, i.Quantity, i.Price * i.Quantity)).ToList());
        }
    }
}
