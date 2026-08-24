using MediatR;
using zizo_shop.Application.DTOs.Coupon;

namespace zizo_shop.Application.Features.Coupons.Commands
{
    public record ApplyCouponCommand(string Code) : IRequest<ApplyCouponResultDto>;

}
