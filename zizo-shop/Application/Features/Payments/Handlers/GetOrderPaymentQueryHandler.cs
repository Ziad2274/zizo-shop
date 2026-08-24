using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Payment;
using zizo_shop.Application.Features.Payments.Queries;

namespace zizo_shop.Application.Features.Payments.Handlers
{
    public class GetOrderPaymentQueryHandler : IRequestHandler<GetOrderPaymentQuery, PaymentDto?>
    {
        private readonly IApplicationDbContext _context;
        public GetOrderPaymentQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        // Change the .Select to use the PaymentDto constructor instead of object initializer
        public async Task<PaymentDto?> Handle(GetOrderPaymentQuery request, CancellationToken cancellationToken)
        => await _context.Payments
            .Where(p => p.OrderId == request.OrderId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PaymentDto(
                p.Id,
                p.OrderId,
                p.Amount,
                p.Status.ToString(),
                p.Provider,
                p.PaymentIntentId,
                p.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
