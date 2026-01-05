using Microsoft.EntityFrameworkCore;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        internal DbSet<Product> Products { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
