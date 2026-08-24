using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Coupons.Commands;

namespace zizo_shop.Application.Features.Coupons.Handlers
{
    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand>
    {
        private readonly IApplicationDbContext _context;    
        public DeleteCouponCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons.FindAsync(new object[] {request.Id }, cancellationToken) ?? throw new KeyNotFoundException("Coupon not found.");

            _context.Coupons.Remove(coupon);    
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
