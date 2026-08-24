using MediatR;
using zizo_shop.Application.DTOs.Coupon;

namespace zizo_shop.Application.Features.Coupons.Queries
{
    public record GetCouponsQuery() : IRequest<List<CouponDto>>;

}
