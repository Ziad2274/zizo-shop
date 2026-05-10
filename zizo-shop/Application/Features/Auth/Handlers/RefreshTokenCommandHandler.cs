using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public RefreshTokenCommandHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var stored = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == request.Token, cancellationToken);

            if (stored == null || stored.IsRevoked || stored.Expires < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(stored.UserId.ToString())
                ?? throw new UnauthorizedAccessException("User not found.");

            stored.IsRevoked = true;

            var newRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync(cancellationToken);

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

            return new AuthResponseDto(
                new JwtSecurityTokenHandler().WriteToken(jwtToken),
                newRefreshToken.Token);
        }
    }
}