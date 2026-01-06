using Microsoft.EntityFrameworkCore;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
        DbSet<Order> Orders { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
