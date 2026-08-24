using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Coupons.Commands;

namespace zizo_shop.Application.Features.Coupons.Handlers
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        public CreateCouponCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var code = request.Code.ToUpper();
            var exists = await _context.Coupons.AnyAsync(c => c.Code == code, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException("Coupon code already exists.");
            }
            var coupon = new Domain.Entities.Coupon
            {
                Code = code,
                DiscountPercent = request.DiscountPercent,
                MaxDiscountAmount = request.MaxDiscountAmount,
                MinOrderAmount = request.MinOrderAmount,
                MaxUses = request.MaxUses,
                ExpiresAt = request.ExpiresAt,
                IsActive = true,
            };
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync(cancellationToken);
            return coupon.Id;
        }
    }
}
