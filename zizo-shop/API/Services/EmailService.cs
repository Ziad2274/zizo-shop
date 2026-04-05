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
            var smtpHost = new SmtpClient(_configuration["Email:Smtp"], int.Parse(_configuration["Email:Port"]))
            {
                Credentials = new System.Net.NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                EnableSsl = true
            };


            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            await smtpHost.SendMailAsync(mailMessage);


        }
    }
}
