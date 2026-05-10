using Microsoft.EntityFrameworkCore;
using zizo_shop.Infrastructure.Data;

namespace zizo_shop.Infrastructure.Jobs
{
    public class CleanupJobs
    {
        private readonly ApplicationDbContext _context;

        public CleanupJobs(ApplicationDbContext context) { _context = context; }

        public async Task RemoveEmptyCarts()
        {
            // Only remove carts with no matching user (orphaned) — NOT empty carts
            // Every user has one permanent cart, emptying it on checkout is correct
            var orphanedCarts = await _context.Carts
                .Where(c => !_context.Users.Any(u => u.Id == c.UserId))
                .ToListAsync();

            if (orphanedCarts.Any())
            {
                _context.Carts.RemoveRange(orphanedCarts);
                await _context.SaveChangesAsync();
            }
        }
    }
}
