using Microsoft.EntityFrameworkCore;
using zizo_shop.Domain.Entities;
using zizo_shop.Infrastructure.Identity; // If RefreshToken is here

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

        // Core Methods
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}