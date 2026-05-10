using MediatR;
using Microsoft.AspNetCore.Identity;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Auth;
using zizo_shop.Application.Features.Auth.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetProfileQueryHandler(ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId.ToString());
            if (user == null) throw new KeyNotFoundException("User not found.");
            return new ProfileDto(user.FirstName, user.LastName, user.Email!, user.PhoneNumber!);
        }
    }
}
