using MediatR;
using Microsoft.AspNetCore.Identity;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class ChangeUserRoleCommandHandler:IRequestHandler<ChangeUserRoleCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ChangeUserRoleCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            var validRoles = new[] { "Admin", "User" };
            if (!validRoles.Contains(request.NewRole))
                throw new ArgumentException(
                    $"Invalid role. Must be one of: {string.Join(", ", validRoles)}");
            var user = await _userManager.FindByIdAsync(request.UserId.ToString())
                ?? throw new KeyNotFoundException("User not found.");
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, request.NewRole);
        }
    }
}
