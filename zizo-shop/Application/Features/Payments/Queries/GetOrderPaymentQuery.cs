using MediatR;
using zizo_shop.Application.DTOs.Payment;

namespace zizo_shop.Application.Features.Payments.Queries
{
    public record GetOrderPaymentQuery(Guid OrderId) : IRequest<PaymentDto>;
}
