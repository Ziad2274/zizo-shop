using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Coupon;
using zizo_shop.Application.Features.Coupons.Commands;

namespace zizo_shop.Application.Features.Coupons.Handlers
{
    public class ApplyCouponCommandHandler : IRequestHandler<ApplyCouponCommand, ApplyCouponResultDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public ApplyCouponCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<ApplyCouponResultDto> Handle(ApplyCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons.
              FirstOrDefaultAsync(c => c.Code == request.Code.ToUpper(), cancellationToken) ?? throw new KeyNotFoundException("Coupon not found.");

            var userId = _currentUserService.UserId;

            var cartTotal = await _context.CartItems
                .Include(c => c.Product)
                .Include(c => c.Cart)
                .Where(ci => ci.Cart.UserId == userId)
                .SumAsync(ci => (ci.Product.DiscountPrice ?? ci.Product.Price) * ci.Quantity, cancellationToken);
            if (!coupon.IsValid(cartTotal))
            {
                throw new InvalidOperationException("Coupon is not valid.");
            }
            var discountAmount = coupon.Calculate(cartTotal);
            return new ApplyCouponResultDto(coupon.Code, discountAmount, cartTotal - discountAmount);
        }

    }
}
