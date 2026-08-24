using MediatR;

namespace zizo_shop.Application.Features.Coupons.Commands
{
    public record CreateCouponCommand(
        string Code,
        decimal DiscountPercent,
        decimal? MaxDiscountAmount,
        decimal? MinOrderAmount,
        int MaxUses,
        DateTime ExpiresAt) : IRequest<Guid>;
}
