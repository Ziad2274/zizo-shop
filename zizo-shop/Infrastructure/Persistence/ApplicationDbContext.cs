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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // FIX: TotalPrice is now a real stored column — do NOT ignore it
            // Cart uses navigation property access (not private backing field)
            builder.Entity<Cart>().Navigation(c => c.Items).UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Entity<Order>().Property(o => o.ShippingFee).HasColumnType("decimal(18,2)");
            builder.Entity<Order>().Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.Entity<Order>().Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            builder.Entity<OrderItem>().Property(oi => oi.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().Property(p => p.DiscountPrice).HasColumnType("decimal(18,2)");

            builder.Entity<Product>().HasOne(p => p.Brand).WithMany(b => b.Products).HasForeignKey(p => p.BrandId);
            builder.Entity<ProductImage>().HasOne(i => i.Product).WithMany(p => p.Images).HasForeignKey(i => i.ProductId);
            builder.Entity<WishlistItem>().HasIndex(w => new { w.UserId, w.ProductId }).IsUnique();
            builder.Entity<ApplicationUser>().HasOne(o => o.Cart).WithOne().HasForeignKey<Cart>(c => c.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
              .HasOne(r => r.User)         
              .WithMany()
              .HasForeignKey(r => r.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Address>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Global decimal precision
            var decimals = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));
            foreach (var property in decimals)
                property.SetColumnType("decimal(18,2)");
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
        public DbSet<Address> Address => Set<Address>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Payment> Payments => Set<Payment>();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await base.SaveChangesAsync(cancellationToken);
    }
}
