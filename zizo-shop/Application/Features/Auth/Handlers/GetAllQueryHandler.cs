using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Features.Auth.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class GetAllQueryHandler : IRequestHandler<GetAllUsersQuery,List<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetAllQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<string>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.Select(u => u.Email).ToListAsync(cancellationToken);
            return users;
        }
    }
}
