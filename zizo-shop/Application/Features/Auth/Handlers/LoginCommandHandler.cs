using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Infrastructure.Data;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;
        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
            ,ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null ||!await _userManager.CheckPasswordAsync(user,request.Password) )
                throw new Exception("Invalid email or password.");
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        authSigningKey,
                        SecurityAlgorithms.HmacSha256)
                    
                );
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(1),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);

        }
    }
}
