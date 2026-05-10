using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Auth;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, IApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var days = _configuration.GetValue<int>("Jwt:DurationInDays", 1);
            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(days),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto(
                new JwtSecurityTokenHandler().WriteToken(jwtToken),
                refreshToken.Token);
        }
    }
}
