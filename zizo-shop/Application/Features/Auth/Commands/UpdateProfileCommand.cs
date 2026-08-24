using MediatR;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record UpdateProfileCommand(string FirstName,string LastName,string?Phone): IRequest;
    
}
