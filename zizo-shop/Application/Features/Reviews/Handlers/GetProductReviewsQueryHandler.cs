using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Review;
using zizo_shop.Application.Features.Reviews.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Reviews.Handlers
{
    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, List<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetProductReviewsQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        { _context = context; _userManager = userManager; }

        public async Task<List<ReviewDto>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
            => await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == request.ProductId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto(
                    r.Id, r.ProductId, r.UserId,
                    r.User.FirstName + " " + r.User.LastName,
                    r.Rating, r.Comment, r.CreatedAt))
                .ToListAsync(cancellationToken);
    }
}
