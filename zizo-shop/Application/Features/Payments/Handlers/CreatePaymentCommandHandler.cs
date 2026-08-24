using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Payments.Commands;
using zizo_shop.Domain.Entities;
using zizo_shop.Domain.Enums;

namespace zizo_shop.Application.Features.Payments.Handlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreatePaymentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, 
                cancellationToken) ?? throw new KeyNotFoundException("Order not found.");

            if(order.Status!=OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be paid for.");

            var duplicatePayment = await _context.Payments.AnyAsync(p => p.OrderId == request.OrderId, cancellationToken);
            if (duplicatePayment)
                throw new InvalidOperationException("A payment for this order already exists.");

            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = order.TotalPrice,
                Provider = request.Provider,
                Status = PaymentStatus.Pending,
                PaymentIntentId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
                };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return payment.OrderId;
        }
    }
}
