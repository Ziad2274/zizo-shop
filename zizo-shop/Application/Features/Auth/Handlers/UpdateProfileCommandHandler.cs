using MediatR;
using Microsoft.AspNetCore.Identity;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UpdateProfileCommandHandler(ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId.ToString())
             ?? throw new KeyNotFoundException("User not found.");
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update profile: {errors}");

            }
        }
    }
}
