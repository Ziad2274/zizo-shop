using MediatR;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        internal RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<string> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception("Registration failed");

            await _userManager.AddToRoleAsync(user, "User");

            return "User registered successfully";
        }
    }

}
