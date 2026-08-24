using Hangfire;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Infrastructure.Data;

namespace zizo_shop.Infrastructure.Jobs
{
    public class CleanupJobs
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CleanupJobs> _logger;

        public CleanupJobs(ApplicationDbContext context, ILogger<CleanupJobs> logger)
        {
            _context = context;
            _logger = logger;
        }
        [AutomaticRetry(Attempts = 3)] // Don't retry more 3 if it fails, to avoid potential cascading issues
        public async Task RemoveEmptyCarts()
        {
            // Only remove carts with no matching user (orphaned) — NOT empty carts
            // Every user has one permanent cart, emptying it on checkout is correct
            var cutoff = DateTime.UtcNow.AddDays(-7);
            var emptyCarts = await _context.Carts
                .Where(c => !c.Items.Any() && c.CreatedAt < cutoff)
                .ToListAsync();
            if (emptyCarts.Any())
            {
                _context.Carts.RemoveRange(emptyCarts);
                await _context.SaveChangesAsync();
                _logger.LogInformation(
                    "Removed {Count} empty carts that were created before {CutoffDate}.", emptyCarts.Count);
            }
            else
            {
                _logger.LogInformation("No empty carts found that were created before {CutoffDate}.", cutoff);
            }
        }
        [AutomaticRetry(Attempts = 0)]
        public async Task ExpireOldCoupon()
        {
            var expiredCoupons = await _context.Coupons
                .Where(c => c.IsActive && c.ExpiresAt < DateTime.UtcNow && c.IsActive)
                .ToListAsync();
            if (!expiredCoupons.Any())
            {
                foreach (var coupon in expiredCoupons)
                    coupon.IsActive = false;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Expired {Count} coupons that passed their expiration date.", expiredCoupons.Count);
            }
            else
            {
                _logger.LogInformation("No coupons found that are past their expiration date.");
            }
        }
        [AutomaticRetry(Attempts = 3)]
        public async Task RevokeExpiredRefreshTokens()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.Expires < DateTime.UtcNow && !rt.IsRevoked)
                .ToListAsync();
            if (expiredTokens.Any())
            {
                foreach (var token in expiredTokens)
                    token.IsRevoked = true;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Revoked {Count} expired refresh tokens.", expiredTokens.Count);
            }
            else
            {
                _logger.LogInformation("No expired refresh tokens found to revoke.");
            }
        }
        [AutomaticRetry(Attempts = 3)]
        public async Task CancelAbandonedPendingOrders()
        {
            var cutoff = DateTime.UtcNow.AddHours(-24);
            var abandonedOrders = await _context.Orders
                .Where(o => o.Status == Domain.Enums.OrderStatus.Pending && o.CreatedAt < cutoff)
                .ToListAsync();
            var pendingPayments = await _context.Payments
                .Where(p => p.Status == Domain.Enums.PaymentStatus.Pending)
                .Select(p => p.OrderId)
                .ToListAsync();
            var products=await _context.Products.ToListAsync();
            if (abandonedOrders.Any())
            {
                foreach (var order in abandonedOrders)
                {
                    order.Cancel();
                    foreach (var item in order.Items) {
                        var product = products.FirstOrDefault(p => p.Id == item.Id);
                        product?.UpdateStock(item.Quantity);
                    }

                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cancelled {Count} pending orders that were created before {CutoffDate}.", abandonedOrders.Count, cutoff);
            }
            else
            {
                _logger.LogInformation("No pending orders found that were created before {CutoffDate}.", cutoff);
            }
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
