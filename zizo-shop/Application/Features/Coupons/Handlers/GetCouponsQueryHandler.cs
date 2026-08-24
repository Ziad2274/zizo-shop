using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Coupon;
using zizo_shop.Application.Features.Coupons.Queries;

namespace zizo_shop.Application.Features.Coupons.Handlers
{
    public class GetCouponsQueryHandler : IRequestHandler<GetCouponsQuery, List<CouponDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetCouponsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CouponDto>> Handle(GetCouponsQuery request, CancellationToken cancellationToken)
        => await _context.Coupons
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CouponDto(
                    c.Id,
                    c.Code,
                    c.DiscountPercent,
                    c.MaxDiscountAmount,
                    c.MinOrderAmount,
                    c.MaxUses,
                    c.UsedCount,
                    c.ExpiresAt,
                    c.IsActive))
                .ToListAsync(cancellationToken);
    }
}
