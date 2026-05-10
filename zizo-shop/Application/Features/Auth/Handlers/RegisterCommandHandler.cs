using MediatR;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using zizo_shop.Domain.Entities;
using zizo_shop.Application.Common.Interfaces;
namespace zizo_shop.Application.Features.Auth.Handlers

{
    public class RegisterCommandHandler
            : IRequestHandler<RegisterCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configration;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _emailService = emailService;
            _configration = configuration;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Cart = new Domain.Entities.Cart (
                    Guid.NewGuid())



            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception(
                    string.Join(",", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink =
                $"{_configration["App:BaseUrl"]}/api/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";


            await _emailService.SendEmailAsync(
                user.Email!,
                "Confirm your email",
                $"Please confirm your email by clicking on the following link: {confirmationLink}"
                );
        }
    }

}
