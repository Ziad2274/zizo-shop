using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Order;
using zizo_shop.Application.Features.Orders.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Orders.Handlers
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDetailDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetAllOrdersQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        { _context = context; _userManager = userManager; }

        public async Task<List<OrderDetailDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);

            var result = new List<OrderDetailDto>();
            foreach (var o in orders)
            {
                var user = await _userManager.FindByIdAsync(o.UserId.ToString());
                result.Add(new OrderDetailDto(
                    o.Id, o.CreatedAt, o.TotalPrice, o.SubTotal, o.ShippingFee,
                    o.Status.ToString(), user?.Email ?? "unknown", o.AddressId,
                    o.Items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.Price, i.Quantity, i.Price * i.Quantity)).ToList()));
            }
            return result;
        }
    }
}
