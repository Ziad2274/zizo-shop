using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zizo_shop.Application.Features.Auth.Commands;

namespace zizo_shop.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
        }
    }
}
