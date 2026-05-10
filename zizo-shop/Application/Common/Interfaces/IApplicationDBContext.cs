using Microsoft.EntityFrameworkCore;
using zizo_shop.Domain.Entities;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
        DbSet<Brand> Brands { get; }
        DbSet<ProductImage> ProductImages { get; }
        DbSet<Cart> Carts { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<WishlistItem> WishlistItems { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<Address> Address { get; }
        DbSet<Review> Reviews { get; }
        DbSet<Payment> Payments { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
