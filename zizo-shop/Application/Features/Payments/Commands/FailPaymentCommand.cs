using MediatR;

namespace zizo_shop.Application.Features.Payments.Commands
{
    public record FailPaymentCommand(Guid PaymentId):IRequest;
}
