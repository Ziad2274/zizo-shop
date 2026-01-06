using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using zizo_shop.Application.DTOs.Auth;
using zizo_shop.Application.DTOs.User;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Application.Features.Auth.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _usermanager;
        public AuthController(IMediator mediator,UserManager<ApplicationUser>userManager)
        {
            _mediator = mediator;
            _usermanager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var command = new RegisterCommand(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.Phone,
                dto.Password
            );
            await _mediator.Send(command);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginData)
        {
            var token = await _mediator.Send(new LoginCommand(loginData.Email, loginData.Password));
            return Ok(new AuthResponseDto(token));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            var user=_usermanager.FindByIdAsync(userId.ToString());
            if (user == null)  return BadRequest("Invalid User Id");
            
            var result= await _usermanager.ConfirmEmailAsync(user.Result, token);
            if (!result.Succeeded) return BadRequest("Email confirmation failed");
            //  await _mediator.Send(new ConfirmEmailCommand(userId, token));
            return Ok("Email confirmed successfully");
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            return Ok(await _mediator.Send(new GetProfileQuery()));
        }

    }
}
