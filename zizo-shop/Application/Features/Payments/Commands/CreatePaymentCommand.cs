using MediatR;

namespace zizo_shop.Application.Features.Payments.Commands
{
    public record CreatePaymentCommand
    (
        
        Guid OrderId,
        string Provider
    ) : IRequest<Guid>;
}
