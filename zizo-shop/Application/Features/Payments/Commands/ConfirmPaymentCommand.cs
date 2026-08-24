using MediatR;

namespace zizo_shop.Application.Features.Payments.Commands
{
    public record ConfirmPaymentCommand
    (Guid PaymentId): IRequest;
}
