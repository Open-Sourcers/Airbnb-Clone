
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
        private readonly IOptions<MailSettings> _options;

        public MailService(IOptions<MailSettings> options)
        {
            _options = options;
        }
        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachedFiles = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Value.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));
            var builder = new BodyBuilder();

            if(attachedFiles != null)
            {
                byte[] filesBytes;

                foreach (var file in attachedFiles)
                {
                    if(file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        filesBytes = ms.ToArray();
                        builder.Attachments.Add(file.FileName,  filesBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));

            using var smtp = new SmtpClient();

            smtp.Connect(_options.Value.Host, _options.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
