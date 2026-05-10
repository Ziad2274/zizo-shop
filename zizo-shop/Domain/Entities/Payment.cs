using zizo_shop.Domain.Enums;

namespace zizo_shop.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string Provider { get; set; } = null!;
        public string? PaymentIntentId { get; set; }
    }
}
