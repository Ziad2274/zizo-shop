using System.Net;
using System.Net.Mail;
using zizo_shop.Application.Common.Interfaces;

namespace zizo_shop.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp     = _configuration["Email:Smtp"]     ?? throw new InvalidOperationException("Email:Smtp missing");
            var portStr  = _configuration["Email:Port"]     ?? throw new InvalidOperationException("Email:Port missing");
            var username = _configuration["Email:Username"] ?? throw new InvalidOperationException("Email:Username missing");
            var password = _configuration["Email:Password"] ?? throw new InvalidOperationException("Email:Password missing");
            var from     = _configuration["Email:From"]     ?? throw new InvalidOperationException("Email:From missing");

            if (!int.TryParse(portStr, out int port))
                throw new InvalidOperationException("Email:Port is not a valid integer");

            using var message = new MailMessage(from, to, subject, body) { IsBodyHtml = true };

            using var client = new SmtpClient(smtp, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
