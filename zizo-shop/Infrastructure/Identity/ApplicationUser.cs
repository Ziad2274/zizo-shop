using Microsoft.AspNetCore.Identity;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Cart Cart { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
