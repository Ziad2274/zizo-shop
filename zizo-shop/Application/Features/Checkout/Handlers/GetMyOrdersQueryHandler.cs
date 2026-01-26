using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Order;
using zizo_shop.Application.Features.Checkout.Queries;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Checkout.Handlers
{
    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, List<OrderDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        
        public GetMyOrdersQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<List<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o=>new OrderDto(
                    o.Id,
                    o.CreatedAt,
                    o.TotalPrice,
                    o.SubTotal,
                    o.ShippingFee,
                    o.Status.ToString(),
                    o.Items.Count
                    )  )
                .ToListAsync(cancellationToken)
                 ;
        }
    }
}
