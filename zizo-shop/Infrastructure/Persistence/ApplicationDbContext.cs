using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Domain.Entities;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Infrastructure.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>,
          IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>().Ignore(o=> o.TotalPrice);
           
            // 2. Configure the backing field for Cart Items (Encapsulation)
            builder.Entity<Cart>().Navigation(c => c.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Entity<Order>().Property(o => o.ShippingFee).HasColumnType("decimal(18,2)");
            builder.Entity<Order>().Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.Entity<OrderItem>().Property(oi => oi.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().Property(p => p.DiscountPrice).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().HasOne(p => p.Brand).WithMany(b => b.Products).HasForeignKey(p => p.BrandId);
            builder.Entity<ProductImage>().HasOne(i => i.Product).WithMany(p => p.Images).HasForeignKey(i => i.ProductId);
            builder.Entity<WishlistItem>().HasIndex(w => new { w.UserId, w.ProductId }).IsUnique();
            builder.Entity<ApplicationUser>().HasOne(o=>o.Cart).WithOne().HasForeignKey<Cart>(c=>c.UserId).OnDelete(DeleteBehavior.Cascade);
            var decimals = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));
            foreach (var property in decimals)
                {
                property.SetColumnType("decimal(18,2)");
            }

        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
            => await base.SaveChangesAsync(cancellationToken);
    }
}
