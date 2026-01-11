using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.DTOs.Auth;
using zizo_shop.Application.DTOs.User;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Application.Features.Auth.Queries;
using zizo_shop.Infrastructure.Identity;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(IMediator mediator, UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await _mediator.Send(new RegisterCommand(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.Phone,
            dto.Password
        ));

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _mediator.Send(
            new LoginCommand(dto.Email, dto.Password));

        return Ok(new AuthResponseDto(token));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _mediator.Send(new GetAllUsersQuery()));
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return BadRequest("Invalid User Id");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded) return BadRequest("Email confirmation failed");

        return Ok("Email confirmed successfully");
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        return Ok(await _mediator.Send(new GetProfileQuery()));
    }

    [AllowAnonymous]
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Auth controller is reachable");
    }
}
