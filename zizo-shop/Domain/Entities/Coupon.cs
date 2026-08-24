namespace zizo_shop.Domain.Entities
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; } = null!;
        public decimal DiscountPercent { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int MaxUses { get; set; }
        public int UsedCount { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;

        public bool IsValid(decimal orderTotal)
        {
            if (!IsActive) return false;
            if (DateTime.UtcNow > ExpiresAt) return false;
            if (UsedCount >= MaxUses) return false;
            if (MinOrderAmount.HasValue && orderTotal < MinOrderAmount) return false;
            return true;
        }

        public decimal Calculate(decimal orderTotal)
        {
            var discount = orderTotal * (DiscountPercent / 100m);

            if (MaxDiscountAmount.HasValue && discount > MaxDiscountAmount)
                discount = MaxDiscountAmount.Value;

            return Math.Round(discount, 2);
        }

        public void IncrementUsage() => UsedCount++;
    }
}