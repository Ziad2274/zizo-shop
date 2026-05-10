
namespace zizo_shop.Domain.Entities
{
    public class WishlistItem : BaseEntity
    {
        public Guid UserId { get; set; } 

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }

}
