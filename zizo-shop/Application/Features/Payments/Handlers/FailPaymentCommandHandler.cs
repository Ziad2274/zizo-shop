using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Payments.Commands;

namespace zizo_shop.Application.Features.Payments.Handlers
{
    public class FailPaymentCommandHandler : IRequestHandler<FailPaymentCommand>
    {
        private readonly IApplicationDbContext _context;
        public FailPaymentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async
            Task Handle(FailPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment =await _context.Payments.FindAsync(request.PaymentId)
                ?? throw new KeyNotFoundException("Payment not found.");

            if (payment.Status != Domain.Enums.PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be marked as failed.");
            payment.Status = Domain.Enums.PaymentStatus.Failed;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
