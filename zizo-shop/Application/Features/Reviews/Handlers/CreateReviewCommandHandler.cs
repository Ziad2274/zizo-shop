using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Reviews.Commands;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Reviews.Handlers
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CreateReviewCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        { _context = context; _currentUserService = currentUserService; }

        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            if (request.Rating < 1 || request.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var productExists = await _context.Products.AnyAsync(p => p.Id == request.ProductId, cancellationToken);
            if (!productExists) throw new KeyNotFoundException("Product not found.");

            var userId = _currentUserService.UserId;
            var alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.ProductId == request.ProductId && r.UserId == userId, cancellationToken);
            if (alreadyReviewed) throw new InvalidOperationException("You have already reviewed this product.");

            var review = new Review
            {
                UserId = userId,
                ProductId = request.ProductId,
                Rating = request.Rating,
                Comment = request.Comment
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);
            return review.Id;
        }
    }
}
