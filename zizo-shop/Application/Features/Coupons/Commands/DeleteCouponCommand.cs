using MediatR;

namespace zizo_shop.Application.Features.Coupons.Commands
{
    public record DeleteCouponCommand(Guid Id) : IRequest;

}
