using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Reviews.Commands;

namespace zizo_shop.Application.Features.Reviews.Handlers
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public DeleteReviewCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        { _context = context; _currentUserService = currentUserService; }

        public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var review = await _context.Reviews.FindAsync(new object[] { request.ReviewId }, cancellationToken)
                ?? throw new KeyNotFoundException("Review not found.");

            // User can delete their own; admin can delete any (admin check is in controller)
            if (review.UserId != userId)
                throw new UnauthorizedAccessException("You can only delete your own reviews.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
