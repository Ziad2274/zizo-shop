using MediatR;
using zizo_shop.Application.DTOs.Auth;

namespace zizo_shop.Application.Features.Auth.Queries
{
    public class GetProfileQuery:IRequest<ProfileDto>
    {
    }
}
