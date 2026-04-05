using zizo_shop.Infrastructure.Data;

namespace zizo_shop.Infrastructure.Jobs
{
    public class CleanupJobs
    {
        private readonly ApplicationDbContext _context;
       public CleanupJobs(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task RemoveEmptyCarts ()
        {
            var emptyCarts = _context.Carts.Where(c => !c.Items.Any());
            _context.Carts.RemoveRange(emptyCarts);
            await _context.SaveChangesAsync();
        }

    }
}
