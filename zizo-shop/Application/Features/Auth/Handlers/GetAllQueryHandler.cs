using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.DTOs.Auth;
using zizo_shop.Application.Features.Auth.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class GetAllQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken);

            var result = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserDto(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email!,
                    user.PhoneNumber,
                    roles
                ));
            }
            return result;
        }
    }
}
