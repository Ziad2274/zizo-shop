namespace zizo_shop.Application.DTOs.Dashboard
{
    public record DashboardStatsDto(
        int TotalUsers,
        int TotalProducts,
        int TotalOrders,
        int PendingOrders,
        int TotalCategories,
        int TotalBrands,
        decimal TotalRevenue,
        decimal RevenueThisMonth,
        int OrdersThisMonth,
        int LowStockProducts,
        List<TopProductDto> TopProducts,
        List<RecentOrderDto> RecentOrders
    );

    public record TopProductDto(
        Guid Id,
        string Name,
        int ReviewCount,
        double AverageRating,
        int Stock,
        decimal Price);

    public record RecentOrderDto(
        Guid Id,
        DateTime CreatedAt,
        decimal Total,
        string Status,
        string UserEmail);
}