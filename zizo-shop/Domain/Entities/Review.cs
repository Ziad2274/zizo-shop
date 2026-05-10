using System.ComponentModel.DataAnnotations.Schema;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
