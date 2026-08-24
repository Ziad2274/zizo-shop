namespace zizo_shop.Application.DTOs.Coupon
{
    public record CouponDto(
        Guid Id,
        string Code,
        decimal DiscountPercent,
        decimal? MaxDiscountAmount,
        decimal? MinOrderAmount,
        int MaxUses,
        int UsedCount,
        DateTime ExpiresAt,
        bool IsActive);

    public record ApplyCouponResultDto(
        string Code,
        decimal DiscountAmount,
        decimal NewTotal);
}