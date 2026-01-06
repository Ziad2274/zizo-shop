using System.Security.Claims;
using zizo_shop.Application.Common.Interfaces;

namespace zizo_shop.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId =>
            Guid.Parse(
                _httpContextAccessor.HttpContext!
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!
            );

    }
}
