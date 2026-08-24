using MediatR;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record ChangePasswordCommand(string CurrentPassword,string NewPassword): IRequest;
   
}
