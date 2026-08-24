using MediatR;
using zizo_shop.Application.DTOs.Dashboard;

namespace zizo_shop.Application.Features.Dashboard.Queries
{
    public record GetDashboardStatsQuery() : IRequest<DashboardStatsDto>;

}
