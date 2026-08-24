using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Payments.Commands;

namespace zizo_shop.Application.Features.Payments.Handlers
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand>
    {
        private readonly IApplicationDbContext _context;
        public ConfirmPaymentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments
                  .FindAsync(  request.PaymentId , cancellationToken)
                  ?? throw new KeyNotFoundException("Payment not found.");
            if (payment.Status != Domain.Enums.PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be confirmed.");
            payment.Status = Domain.Enums.PaymentStatus.Completed;
            var order = await _context.Orders.FindAsync(new object[] { payment.OrderId }, cancellationToken) ?? throw new KeyNotFoundException("Order not found.");
            order?.MarkAsPaid();
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
