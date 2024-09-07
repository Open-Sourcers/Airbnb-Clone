using Airbnb.Application.Settings;
using Airbnb.Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Airbnb.Application.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _options;

        public MailService(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachedFiles = null)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_options.Email);
            email.To.Add(MailboxAddress.Parse(mailTo));
            email.Subject = subject;
            var builder = new BodyBuilder();
            if (attachedFiles != null)
            {
                byte[] fileBytes;
                foreach (var file in attachedFiles)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
