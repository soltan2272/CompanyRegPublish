using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CompanyServiceLayer.Interfaces;
using Microsoft.Extensions.Options;
using CompanyServiceLayer.Configrations;

namespace CompanyServiceLayer.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtp;

        public EmailService(IOptions<SmtpSettings> smtp)
        {
            _smtp = smtp.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                EnableSsl = _smtp.EnableSsl,
                Credentials = new NetworkCredential(_smtp.UserName, _smtp.Password)
            };

            var message = new MailMessage(_smtp.UserName, to, subject, body);
            await client.SendMailAsync(message);
        }
    }

}
