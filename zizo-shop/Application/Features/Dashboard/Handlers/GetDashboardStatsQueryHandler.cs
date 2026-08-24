using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Dashboard;
using zizo_shop.Application.Features.Dashboard.Queries;
using zizo_shop.Domain.Enums;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Dashboard.Handlers
{
    public class GetDashboardStatsQueryHandler
        : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetDashboardStatsQueryHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<DashboardStatsDto> Handle(
            GetDashboardStatsQuery request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            // ── Counts ────────────────────────────────────────────────────────
            var totalUsers = await _userManager.Users.CountAsync(cancellationToken);
            var totalProducts = await _context.Products.CountAsync(cancellationToken);
            var totalOrders = await _context.Orders.CountAsync(cancellationToken);
            var totalCats = await _context.Categories.CountAsync(cancellationToken);
            var totalBrands = await _context.Brands.CountAsync(cancellationToken);
            var lowStock = await _context.Products
                                    .CountAsync(p => p.StockQuantity < 10, cancellationToken);

            var pendingOrders = await _context.Orders
                                    .CountAsync(
                                        o => o.Status == OrderStatus.Pending,
                                        cancellationToken);

            // ── Revenue ───────────────────────────────────────────────────────
            var revenueStatuses = new[]
            {
                OrderStatus.Paid,
                OrderStatus.Shipped,
                OrderStatus.Delivered
            };

            var totalRevenue = await _context.Orders
                .Where(o => revenueStatuses.Contains(o.Status))
                .SumAsync(o => o.TotalPrice, cancellationToken);

            var revenueThisMonth = await _context.Orders
                .Where(o => revenueStatuses.Contains(o.Status)
                         && o.CreatedAt >= monthStart)
                .SumAsync(o => o.TotalPrice, cancellationToken);

            var ordersThisMonth = await _context.Orders
                .CountAsync(o => o.CreatedAt >= monthStart, cancellationToken);

            // ── Top 5 products by review count ────────────────────────────────
            var topProducts = await _context.Products
                .Include(p => p.Reviews)
                .OrderByDescending(p => p.Reviews.Count)
                .Take(5)
                .Select(p => new TopProductDto(
                    p.Id,
                    p.Name,
                    p.Reviews.Count,
                    p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    p.StockQuantity,
                    p.DiscountPrice ?? p.Price))
                .ToListAsync(cancellationToken);

            // ── 10 most recent orders ─────────────────────────────────────────
            // Batch load to avoid N+1
            var recentOrdersRaw = await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .ToListAsync(cancellationToken);

            var userIds = recentOrdersRaw
                .Select(o => o.UserId.ToString())
                .Distinct()
                .ToList();

            // Load all needed users in one batch
            var userEmails = new Dictionary<string, string>();
            foreach (var uid in userIds)
            {
                var user = await _userManager.FindByIdAsync(uid);
                userEmails[uid] = user?.Email ?? "unknown";
            }

            var recentOrders = recentOrdersRaw
                .Select(o => new RecentOrderDto(
                    o.Id,
                    o.CreatedAt,
                    o.TotalPrice,
                    o.Status.ToString(),
                    userEmails[o.UserId.ToString()]))
                .ToList();

            return new DashboardStatsDto(
                totalUsers,
                totalProducts,
                totalOrders,
                pendingOrders,
                totalCats,
                totalBrands,
                totalRevenue,
                revenueThisMonth,
                ordersThisMonth,
                lowStock,
                topProducts,
                recentOrders);
        }
    }
}